import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
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
  styleUrls: ['./process-list.component.css']
})
export class ProcessListComponent implements OnInit, OnDestroy {
  processes: ProcessModel[] = [];
  searchTerm: string = '';
  searchControl: FormControl;
  private searchSub?: Subscription;
  // modal state for process modal component
  modalVisible = false;
  modalToEdit?: ProcessModel | null;
  // paging / infinite scroll
  pageSize = 20;
  currentPage = 0; // 0 means not loaded yet
  isLoading = false;
  hasMore = true;

  constructor(
    private processService: ProcessService,
    private router: Router
  ) {
    this.searchControl = new FormControl(this.searchTerm);
  }

  ngOnInit(): void {
    this.loadPage(1);
    this.searchSub = this.searchControl.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(value => {
      this.searchTerm = value || '';
      // reset paging and reload
      this.currentPage = 0;
      this.hasMore = true;
      this.processes = [];
      this.loadPage(1);
    });
  }

  private loadPage(page: number): void {
    if (this.isLoading || !this.hasMore) return;
    this.isLoading = true;
    this.processService.getProcesses(this.searchTerm, page, this.pageSize).subscribe(data => {
      const items = data.map(d => ProcessModel.fromDto(d));
      if (page === 1) {
        this.processes = items;
      } else {
        this.processes = this.processes.concat(items);
      }
      this.currentPage = page;
      if (items.length < this.pageSize) {
        this.hasMore = false;
      }
      this.isLoading = false;
    }, () => {
      this.isLoading = false;
    });
  }

  editProcess(id: number): void {
    // open edit modal instead of navigating
    this.openEdit(id);
  }

  deleteProcess(id: number): void {
    if (confirm('Tem certeza que deseja excluir este processo?')) {
      this.processService.deleteProcess(id).subscribe(() => {
        // refresh from first page after delete
        this.currentPage = 0;
        this.hasMore = true;
        this.processes = [];
        this.loadPage(1);
      });
    }
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
    const p = this.processes.find(x => x.id === id);
    if (p) {
      this.modalToEdit = p;
      this.modalVisible = true;
      return;
    }
    // fallback: load from server then open
    this.processService.getProcessById(id).subscribe(proc => {
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
    // refresh from first page
    this.currentPage = 0;
    this.hasMore = true;
    this.processes = [];
    this.loadPage(1);
  }

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    if (this.isLoading || !this.hasMore) return;
    const threshold = 300; // px
    const position = window.innerHeight + window.scrollY;
    const height = document.documentElement.scrollHeight;
    if (position >= height - threshold) {
      this.loadPage(this.currentPage + 1);
    }
  }
}
