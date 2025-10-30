import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Process } from '../models/process.model';

@Injectable({
  providedIn: 'root'
})
export class ProcessService {
  private apiUrl = '/api/Processos';

  constructor(private http: HttpClient) { }

  // searchTerm first (optional), then page and limit
  getProcesses(searchTerm: string = '', page: number = 1, limit: number = 10): Observable<Process[]> {
    const url = `${this.apiUrl}?search=${encodeURIComponent(searchTerm)}&page=${page}&limit=${limit}`;
    return this.http.get<Process[]>(url);
  }

  getProcessById(id: number): Observable<Process> {
    return this.http.get<Process>(`${this.apiUrl}/${id}`);
  }

  createProcess(process: Omit<Process, 'id' | 'historico'>): Observable<Process> {
    return this.http.post<Process>(this.apiUrl, process);
  }

  updateProcess(id: number, process: Partial<Omit<Process, 'id' | 'historico'>>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, process);
  }

  deleteProcess(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  addMovement(processId: number, movement: { descricao: string }): Observable<any> {
    return this.http.post(`/api/processos/${processId}/historicos`, movement);
  }

}
