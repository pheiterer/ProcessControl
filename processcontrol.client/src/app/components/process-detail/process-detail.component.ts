import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProcessModel, ProcessHistory } from '../../models/process.model';
// bootstrap is used in shared modal component; not needed here
import { forkJoin } from 'rxjs';
import { ProcessService } from '../../services/process.service';
import { CommonModule } from '@angular/common';
import { MovementFormComponent } from '../movement-form/movement-form.component';
import { ProcessModalComponent } from '../process-modal/process-modal.component';
import { ModalComponent } from '../shared/modal/modal.component';

@Component({
  selector: 'app-process-detail',
  standalone: true,
  imports: [
    CommonModule,
    MovementFormComponent,
    ProcessModalComponent,
    FormsModule,
    ModalComponent,
  ],
  templateUrl: './process-detail.component.html',
  styleUrls: ['./process-detail.component.css'],
})
export class ProcessDetailComponent implements OnInit {
  process: ProcessModel | undefined;
  modalVisible = false;
  historico: ProcessHistory[] = [];
  // movement edit state (legacy inline edit removed)
  editingMovementId: number | null = null;
  editingMovementText = '';
  // pending delete for movement confirmation modal
  pendingDeleteMovementId: number | null = null;
  @ViewChild('confirmMovementModal') confirmMovementModal!: ModalComponent;
  @ViewChild(MovementFormComponent) movementFormComp?: MovementFormComponent;
  // loading flags
  isLoadingProcess = false;
  isLoadingHistorico = false;
  // pagination for historico
  historicoPageSize = 10;
  historicoCurrentPage = 0;
  historicoHasMore = true;

  constructor(
    private route: ActivatedRoute,
    private processService: ProcessService
  ) {}

  ngOnInit(): void {
    this.loadProcess();
  }

  loadProcess(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      const pid = +id;
      // fetch process and first page of historico in parallel
      this.isLoadingProcess = true;
      this.isLoadingHistorico = true;
      forkJoin({
        process: this.processService.getProcessById(pid),
        movements: this.processService.getMovements(pid, 1, this.historicoPageSize),
      }).subscribe(
        ({ process, movements }) => {
          this.process = ProcessModel.fromDto(process);
          this.historicoCurrentPage = 1;
          this.historicoHasMore = movements.length >= this.historicoPageSize;
          this.historico = movements.map((m) => ({
            id: m.id,
            processoId: m.processoId ?? pid,
            descricao: m.descricao,
            dataInclusao: m.dataInclusao ? new Date(m.dataInclusao) : new Date(0),
            dataAlteracao: m.dataAlteracao ? new Date(m.dataAlteracao) : new Date(0),
          }));
          this.isLoadingProcess = false;
          this.isLoadingHistorico = false;
        },
        () => {
          this.isLoadingProcess = false;
          this.isLoadingHistorico = false;
        }
      );
    }
  }

  loadHistorico(processId: number, page = 1, reset = false): void {
    if (reset) {
      this.historico = [];
      this.historicoCurrentPage = 0;
      this.historicoHasMore = true;
    }
    if (!this.historicoHasMore && page > 1) return;
    this.isLoadingHistorico = true;
    this.processService.getMovements(processId, page, this.historicoPageSize).subscribe(
      (movs) => {
        const items = movs.map((m) => ({
          id: m.id,
          processoId: m.processoId ?? processId,
          descricao: m.descricao,
          dataInclusao: m.dataInclusao ? new Date(m.dataInclusao) : new Date(0),
          dataAlteracao: m.dataAlteracao ? new Date(m.dataAlteracao) : new Date(0),
        }));
        if (page === 1) {
          this.historico = items;
        } else {
          this.historico = this.historico.concat(items);
        }
        this.historicoCurrentPage = page;
        this.historicoHasMore = items.length >= this.historicoPageSize;
        this.isLoadingHistorico = false;
      },
      () => {
        this.isLoadingHistorico = false;
      }
    );
  }

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    if (this.isLoadingHistorico || !this.process || !this.historicoHasMore) return;
    const threshold = 300; // px
    const position = window.innerHeight + window.scrollY;
    const height = document.documentElement.scrollHeight;
    if (position >= height - threshold) {
      // load next page
      this.loadHistorico(this.process.id, this.historicoCurrentPage + 1, false);
    }
  }

  // movement edit handlers (on process detail page)
  startEditMovement(m: ProcessHistory): void {
    this.editingMovementId = m.id;
    this.editingMovementText = m.descricao;
  }

  cancelEditMovement(): void {
    this.editingMovementId = null;
    this.editingMovementText = '';
  }

  saveEditMovement(movementId: number): void {
    if (!this.process) return;
    this.isLoadingHistorico = true;
    this.processService
      .updateMovement(this.process.id, movementId, { descricao: this.editingMovementText })
      .subscribe(
        () => {
          // update local historico item: set new description and update dateAlteracao to now
          const idx = this.historico.findIndex((h) => h.id === movementId);
          if (idx >= 0) {
            this.historico[idx].descricao = this.editingMovementText;
            this.historico[idx].dataAlteracao = new Date();
          }
          this.isLoadingHistorico = false;
          this.cancelEditMovement();
        },
        () => {
          this.isLoadingHistorico = false;
        }
      );
  }

  // open confirmation modal for movement deletion
  deleteMovementConfirmed(movementId: number): void {
    if (!this.process) return;
    this.pendingDeleteMovementId = movementId;
    this.confirmMovementModal?.show();
  }

  confirmDeleteMovement(): void {
    if (!this.process || !this.pendingDeleteMovementId) return;
    const id = this.pendingDeleteMovementId;
    this.isLoadingHistorico = true;
    this.processService.deleteMovement(this.process.id, id).subscribe(
      () => {
        this.loadHistorico(this.process!.id, 1, true);
        this.isLoadingHistorico = false;
        this.confirmMovementModal?.hide();
        this.pendingDeleteMovementId = null;
      },
      () => {
        this.isLoadingHistorico = false;
      }
    );
  }

  openEdit(): void {
    this.modalVisible = true;
  }

  closeModal(): void {
    this.modalVisible = false;
  }

  onModalSaved(): void {
    this.closeModal();
    // reload only the process DTO (no need to reload movements/historico)
    if (this.process) {
      this.isLoadingProcess = true;
      this.processService.getProcessById(this.process.id).subscribe(
        (proc) => {
          this.process = ProcessModel.fromDto(proc);
          this.isLoadingProcess = false;
        },
        () => {
          this.isLoadingProcess = false;
        }
      );
    } else {
      // fallback: nothing loaded yet, load everything
      this.loadProcess();
    }
  }
}
