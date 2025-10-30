import { Component, EventEmitter, Input, OnInit, Output, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../shared/modal/modal.component';
import { ProcessService } from '../../services/process.service';
declare var bootstrap: any;

@Component({
  selector: 'app-movement-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalComponent],
  templateUrl: './movement-form.component.html',
  styleUrls: ['./movement-form.component.css']
})
export class MovementFormComponent implements OnInit {
  @Input() processId!: number;
  @Output() movementAdded = new EventEmitter<void>();
  movementForm!: FormGroup;
  @ViewChild('movementModal') movementModal!: ModalComponent;

  constructor(private fb: FormBuilder, private processService: ProcessService) { }

  ngOnInit(): void {
    this.movementForm = this.fb.group({
      descricao: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.movementForm.valid) {
      this.processService.createMovement(this.processId, this.movementForm.value).subscribe(() => {
        this.movementAdded.emit();
        this.movementForm.reset();
        this.movementModal?.hide();
      });
    }
  }

  // allow parent to open the modal programmatically
  show(): void {
    this.movementModal?.show();
  }
}
