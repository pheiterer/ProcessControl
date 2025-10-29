import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Process } from '../../models/process.model';
import { ProcessService } from '../../services/process.service';

@Component({
  selector: 'app-process-list',
  standalone: false,
  templateUrl: './process-list.component.html',
  styleUrls: ['./process-list.component.css']
})
export class ProcessListComponent implements OnInit {
  processes: Process[] = [];
  searchTerm: string = '';

  constructor(private processService: ProcessService, private router: Router) { }

  ngOnInit(): void {
    this.loadProcesses();
  }

  loadProcesses(): void {
    this.processService.getProcesses(this.searchTerm).subscribe(data => {
      this.processes = data;
    });
  }

  editProcess(id: number): void {
    this.router.navigate(['/processes/edit', id]);
  }

  deleteProcess(id: number): void {
    if (confirm('Tem certeza que deseja excluir este processo?')) {
      this.processService.deleteProcess(id).subscribe(() => {
        this.loadProcesses(); // Recarrega a lista
      });
    }
  }

  viewProcess(id: number): void {
    this.router.navigate(['/processes', id]);
  }


  search(): void {
    this.loadProcesses();
  }
}
