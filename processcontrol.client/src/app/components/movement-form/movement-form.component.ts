import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProcessService } from '../../services/process.service';
declare var bootstrap: any;

@Component({
  selector: 'app-movement-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './movement-form.component.html',
  styleUrls: ['./movement-form.component.css']
})
export class MovementFormComponent implements OnInit {
  @Input() processId!: number;
  @Output() movementAdded = new EventEmitter<void>();
  movementForm!: FormGroup;

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
        const modalElement = document.getElementById('movementFormModal');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();
      });
    }
  }
}
