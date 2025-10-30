import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Process, ProcessModel, ProcessStatusText } from '../../models/process.model';
import { ProcessService } from '../../services/process.service';
import { CommonModule } from '@angular/common';
import { MovementFormComponent } from '../movement-form/movement-form.component';
import { ProcessModalComponent } from '../process-modal/process-modal.component';

@Component({
  selector: 'app-process-detail',
  standalone: true,
  imports: [CommonModule, MovementFormComponent, ProcessModalComponent],
  templateUrl: './process-detail.component.html',
  styleUrls: ['./process-detail.component.css']
})
export class ProcessDetailComponent implements OnInit {
  process: ProcessModel | undefined;
  modalVisible = false;

  constructor(
    private route: ActivatedRoute,
    private processService: ProcessService
  ) { }

  ngOnInit(): void {
    this.loadProcess();
  }

  loadProcess(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.processService.getProcessById(+id).subscribe(data => {
        this.process = ProcessModel.fromDto(data);
      });
    }
  }

  openEdit(): void {
    this.modalVisible = true;
  }

  closeModal(): void {
    this.modalVisible = false;
  }

  onModalSaved(): void {
    this.closeModal();
    this.loadProcess();
  }
}