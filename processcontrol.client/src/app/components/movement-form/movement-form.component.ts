import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../shared/modal/modal.component';
import { ProcessService } from '../../services/process.service';

@Component({
  selector: 'app-movement-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalComponent],
  templateUrl: './movement-form.component.html',
  styleUrls: ['./movement-form.component.css'],
})
export class MovementFormComponent implements OnInit {
  @Input() processId!: number;
  @Output() movementAdded = new EventEmitter<void>();
  @Output() movementSaved = new EventEmitter<void>();
  movementForm!: FormGroup;
  @ViewChild('movementModal') movementModal!: ModalComponent;
  // optional movement being edited
  editingMovementId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private processService: ProcessService
  ) {}

  ngOnInit(): void {
    this.movementForm = this.fb.group({
      descricao: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.movementForm.valid) {
      const payload = this.movementForm.value;
      if (this.editingMovementId) {
        // update existing movement
        this.processService
          .updateMovement(this.processId, this.editingMovementId, payload)
          .subscribe(() => {
            this.movementSaved.emit();
            this.movementForm.reset();
            this.editingMovementId = null;
            this.movementModal?.hide();
          });
      } else {
        // create new
        this.processService.createMovement(this.processId, payload).subscribe(() => {
          this.movementAdded.emit();
          this.movementSaved.emit();
          this.movementForm.reset();
          this.movementModal?.hide();
        });
      }
    }
  }

  // allow parent to open the modal programmatically
  show(): void {
    this.movementModal?.show();
  }

  // open modal to edit a specific movement
  openForEdit(movement: { id: number; descricao: string }): void {
    this.editingMovementId = movement.id;
    this.movementForm.patchValue({ descricao: movement.descricao });
    this.movementModal?.show();
  }
}
