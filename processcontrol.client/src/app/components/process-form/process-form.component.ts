import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProcessService } from '../../services/process.service';

@Component({
  selector: 'app-process-form',
  standalone: false,
  templateUrl: './process-form.component.html',
  styleUrls: ['./process-form.component.css']
})
export class ProcessFormComponent implements OnInit {
  processForm: FormGroup;
  isEditMode = false;
  processId?: number;

  constructor(
    private fb: FormBuilder,
    private processService: ProcessService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.processForm = this.fb.group({
      numeroProcesso: ['', Validators.required],
      autor: ['', Validators.required],
      reu: ['', Validators.required],
      dataAjuizamento: ['', Validators.required],
      status: ['Em andamento', Validators.required],
      descricao: ['']
    });
  }

  ngOnInit(): void {
    this.processId = this.route.snapshot.params['id'];
    if (this.processId) {
      this.isEditMode = true;
      this.processService.getProcessById(this.processId).subscribe(process => {
        // O backend deve retornar a data no formato YYYY-MM-DD
        const formattedDate = new Date(process.dataAjuizamento).toISOString().split('T')[0];
        this.processForm.patchValue({ ...process, dataAjuizamento: formattedDate });
      });
    }
  }

  onSubmit(): void {
    if (this.processForm.invalid) {
      return;
    }

    const formValue = this.processForm.value;

    if (this.isEditMode && this.processId) {
      this.processService.updateProcess(this.processId, formValue).subscribe(() => {
        this.router.navigate(['/processes']);
      });
    } else {
      this.processService.createProcess(formValue).subscribe(() => {
        this.router.navigate(['/processes']);
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/processes']);
  }
}
