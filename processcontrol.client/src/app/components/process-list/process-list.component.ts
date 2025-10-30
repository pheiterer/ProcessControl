import { Component, OnInit, OnDestroy, HostListener, ViewChild } from '@angular/core';
import { ModalComponent } from '../shared/modal/modal.component';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { ProcessModel } from '../../models/process.model';
import { ProcessService } from '../../services/process.service';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-process-list',
  standalone: false,
  templateUrl: './process-list.component.html',
  styleUrls: ['./process-list.component.css'],
})
export class ProcessListComponent implements OnInit, OnDestroy {
  processes: ProcessModel[] = [];
  searchTerm = '';
  searchControl: FormControl;
  private searchSub?: Subscription;
  modalVisible = false;
  modalToEdit?: ProcessModel | null;
  pendingDeleteProcessId: number | null = null;

  @ViewChild('confirmProcessModal') confirmProcessModal!: ModalComponent;
  pageSize = 20;
  currentPage = 0;
  isLoading = false;
  hasMore = true;

  // local sorting state
  sortColumn: string | null = null;
  sortAscending = true;

  constructor(
    private processService: ProcessService,
    private router: Router
  ) {
    this.searchControl = new FormControl(this.searchTerm);
  }

  ngOnInit(): void {
    this.loadPage(1);
    this.searchSub = this.searchControl.valueChanges
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe((value) => {
        this.searchTerm = value || '';
        this.currentPage = 0;
        this.hasMore = true;
        this.processes = [];
        this.loadPage(1);
      });
  }

  private loadPage(page: number): void {
    if (this.isLoading || !this.hasMore) return;
    this.isLoading = true;
    this.processService.getProcesses(this.searchTerm, page, this.pageSize).subscribe(
      (data) => {
        const items = data.map((d) => ProcessModel.fromDto(d));
        if (page === 1) {
          this.processes = items;
        } else {
          this.processes = this.processes.concat(items);
        }
        // apply local sort if active
        if (this.sortColumn) {
          this.applySort();
        }
        this.currentPage = page;
        if (items.length < this.pageSize) {
          this.hasMore = false;
        }
        this.isLoading = false;
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  sortBy(column: string): void {
    if (this.sortColumn === column) {
      this.sortAscending = !this.sortAscending;
    } else {
      this.sortColumn = column;
      this.sortAscending = true;
    }
    this.applySort();
  }

  private applySort(): void {
    if (!this.sortColumn) return;
    const col = this.sortColumn;
    this.processes.sort((a, b) => {
      const av = this.getSortableValue(a, col);
      const bv = this.getSortableValue(b, col);
      if (av == null && bv == null) return 0;
      if (av == null) return this.sortAscending ? -1 : 1;
      if (bv == null) return this.sortAscending ? 1 : -1;
      let cmp = 0;
      if (typeof av === 'number' && typeof bv === 'number') {
        cmp = av - bv;
      } else if (av instanceof Date && bv instanceof Date) {
        cmp = av.getTime() - bv.getTime();
      } else {
        cmp = String(av).localeCompare(String(bv), undefined, { sensitivity: 'base' });
      }
      return this.sortAscending ? cmp : -cmp;
    });
  }

  private getSortableValue(item: ProcessModel, column: string): string | number | Date | null {
    switch (column) {
      case 'numeroProcesso':
        return item.numeroProcesso ?? '';
      case 'autor':
        return item.autor ?? '';
      case 'reu':
        return item.reu ?? '';
      case 'dataAjuizamento':
        return item.dataAjuizamento ?? null;
      case 'status':
        return typeof item.status === 'number' ? Number(item.status) : null;
      default:
        return '';
    }
  }

  editProcess(id: number): void {
    this.openEdit(id);
  }

  deleteProcess(id: number): void {
    this.pendingDeleteProcessId = id;
    this.confirmProcessModal?.show();
  }

  confirmDeleteProcess(): void {
    const id = this.pendingDeleteProcessId;
    if (!id) return;
    this.processService.deleteProcess(id).subscribe(() => {
      this.currentPage = 0;
      this.hasMore = true;
      this.processes = [];
      this.loadPage(1);
      this.confirmProcessModal?.hide();
      this.pendingDeleteProcessId = null;
    });
  }

  viewProcess(id: number): void {
    this.router.navigate(['/processes', id]);
  }

  ngOnDestroy(): void {
    this.searchSub?.unsubscribe();
  }

  /* Modal methods (using ProcessModalComponent) */
  openNew(): void {
    this.modalToEdit = null;
    this.modalVisible = true;
  }

  openEdit(id: number): void {
    const p = this.processes.find((x) => x.id === id);
    if (p) {
      this.modalToEdit = p;
      this.modalVisible = true;
      return;
    }
    this.processService.getProcessById(id).subscribe((proc) => {
      this.modalToEdit = ProcessModel.fromDto(proc);
      this.modalVisible = true;
    });
  }

  closeModal(): void {
    this.modalVisible = false;
    this.modalToEdit = undefined;
  }

  onModalSaved(): void {
    this.closeModal();
    this.currentPage = 0;
    this.hasMore = true;
    this.processes = [];
    this.loadPage(1);
  }

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    if (this.isLoading || !this.hasMore) return;
    const threshold = 300;
    const position = window.innerHeight + window.scrollY;
    const height = document.documentElement.scrollHeight;
    if (position >= height - threshold) {
      this.loadPage(this.currentPage + 1);
    }
  }
}
