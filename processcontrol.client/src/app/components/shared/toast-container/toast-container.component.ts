import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { ToastService, ToastMessage } from '../../../services/toast.service';

@Component({
  selector: 'app-toast-container',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      aria-live="polite"
      aria-atomic="true"
      class="position-fixed bottom-0 end-0 p-3"
      style="z-index: 1080;"
    >
      <div
        *ngFor="let m of messages"
        class="toast show align-items-center text-bg-{{ m.type }} border-0 mb-2"
        role="alert"
        aria-live="assertive"
        aria-atomic="true"
      >
        <div class="d-flex">
          <div class="toast-body">{{ m.text }}</div>
          <button
            type="button"
            class="btn-close btn-close-white me-2 m-auto"
            (click)="close(m.id)"
            aria-label="Close"
          ></button>
        </div>
      </div>
    </div>
  `,
  styles: [],
})
export class ToastContainerComponent implements OnInit, OnDestroy {
  messages: ToastMessage[] = [];
  private sub?: Subscription;

  constructor(private toastService: ToastService) {}

  ngOnInit(): void {
    this.sub = this.toastService.messages.subscribe((m) => (this.messages = m));
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  close(id: string): void {
    this.toastService.remove(id);
  }
}
