import { Component, ElementRef, EventEmitter, Input, Output, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  template: `
  <div class="modal fade" tabindex="-1" aria-hidden="true" #modalDiv>
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">{{ title }}</h5>
          <button *ngIf="showClose" type="button" class="btn-close" (click)="onSecondary()" aria-label="Fechar"></button>
        </div>
        <div class="modal-body">
          <ng-content></ng-content>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" (click)="onSecondary()">{{ secondaryText }}</button>
          <button type="button" [ngClass]="['btn', primaryClass]" (click)="onPrimary()">{{ primaryText }}</button>
        </div>
      </div>
    </div>
  </div>
  `,
  styles: [``]
})
export class ModalComponent implements AfterViewInit {
  @Input() title = '';
  @Input() primaryText = 'OK';
  @Input() primaryClass = 'btn-primary';
  @Input() secondaryText = 'Cancelar';
  @Input() showClose = true;

  @Output() primary = new EventEmitter<void>();
  @Output() secondary = new EventEmitter<void>();

  @ViewChild('modalDiv', { static: true }) modalDiv!: ElementRef<HTMLDivElement>;
  private modalInstance: any;

  ngAfterViewInit(): void {
    this.modalInstance = bootstrap.Modal.getOrCreateInstance(this.modalDiv.nativeElement);
  }

  show(): void {
    this.modalInstance?.show();
  }

  hide(): void {
    this.modalInstance?.hide();
  }

  onPrimary(): void {
    this.primary.emit();
  }

  onSecondary(): void {
    this.secondary.emit();
    this.hide();
  }
}
