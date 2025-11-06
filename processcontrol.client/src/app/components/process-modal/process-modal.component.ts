import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
  AfterViewInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../shared/modal/modal.component';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ProcessService } from '../../services/process.service';
import { ProcessModel, ProcessStatus, ProcessStatusText } from '../../models/process.model';

@Component({
  selector: 'app-process-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalComponent],
  templateUrl: './process-modal.component.html',
  styleUrls: ['./process-modal.component.css'],
})
export class ProcessModalComponent implements OnInit, AfterViewInit {
  @Input() process?: ProcessModel | null;
  @Output() saved = new EventEmitter<void>();
  @Output() canceled = new EventEmitter<void>();

  form: FormGroup;
  isEdit = false;
  statusOptions: { value: number; label: string }[] = [];
  @ViewChild('processModal') processModal!: ModalComponent;

  constructor(
    private fb: FormBuilder,
    private processService: ProcessService
  ) {
    this.form = this.fb.group({
      numeroProcesso: ['', [Validators.required, Validators.maxLength(50)]],
      autor: ['', [Validators.required, Validators.maxLength(100)]],
      reu: ['', [Validators.required, Validators.maxLength(100)]],
      dataAjuizamento: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{2}-\d{2}$/)]],
      status: [ProcessStatus.EmAndamento, Validators.required],
      descricao: [''],
    });

    this.statusOptions = Object.keys(ProcessStatusText).map((k) => ({
      value: Number(k),
      label: ProcessStatusText[Number(k)],
    }));
  }

  ngOnInit(): void {
    if (this.process) {
      this.isEdit = true;
      const formattedDate = this.process.dataAjuizamento
        ? new Date(this.process.dataAjuizamento).toISOString().split('T')[0]
        : '';
      this.form.patchValue({
        ...this.process,
        dataAjuizamento: formattedDate,
        status: Number(this.process.status),
      });

      if (Number(this.process.status) === ProcessStatus.Encerrado) {
        this.form.get('status')?.disable();
      }
    }
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.processModal?.show());
  }

  submit(): void {
    if (this.form.invalid) return;
    const fv = this.form.value;
    const payload = {
      ...fv,
      status: Number(fv.status),
      dataAjuizamento: fv.dataAjuizamento ? fv.dataAjuizamento : null,
    };

    if (this.isEdit && this.process && this.process.id) {
      this.processService.updateProcess(this.process.id, payload).subscribe(() => {
        this.processModal?.hide();
        this.saved.emit();
      });
    } else {
      this.processService.createProcess(payload).subscribe(() => {
        this.processModal?.hide();
        this.saved.emit();
      });
    }
  }

  doCancel(): void {
    this.processModal?.hide();
    this.canceled.emit();
  }
}
