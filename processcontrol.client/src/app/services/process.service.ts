import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Process, ProcessHistory } from '../models/process.model';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root',
})
export class ProcessService {
  private apiUrl = '/api/Processos';

  constructor(
    private http: HttpClient,
    private toastService: ToastService
  ) {}

  getProcesses(searchTerm = '', page = 1, limit = 10): Observable<Process[]> {
    const url = `${this.apiUrl}?searchTerm=${encodeURIComponent(searchTerm)}&page=${page}&limit=${limit}`;
    return this.http.get<Process[]>(url).pipe(catchError((err) => this.handleError(err)));
  }

  getProcessById(id: number): Observable<Process> {
    return this.http
      .get<Process>(`${this.apiUrl}/${id}`)
      .pipe(catchError((err) => this.handleError(err)));
  }

  createProcess(process: Omit<Process, 'id' | 'historico'>): Observable<Process> {
    return this.http.post<Process>(this.apiUrl, process).pipe(
      tap(() => this.toastService.show('Processo criado com sucesso.', 'success')),
      catchError((err) => this.handleError(err))
    );
  }

  updateProcess(id: number, process: Partial<Omit<Process, 'id' | 'historico'>>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, process).pipe(
      tap(() => this.toastService.show('Processo atualizado com sucesso.', 'success')),
      catchError((err) => this.handleError(err))
    );
  }

  deleteProcess(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => this.toastService.show('Processo excluído com sucesso.', 'success')),
      catchError((err) => this.handleError(err))
    );
  }

  addMovement(processId: number, movement: { descricao: string }): Observable<ProcessHistory[]> {
    return this.http
      .post<ProcessHistory[]>(`${this.apiUrl}/${processId}/historicos`, movement)
      .pipe(
        tap(() => this.toastService.show('Movimento criado com sucesso.', 'success')),
        catchError((err) => this.handleError(err))
      );
  }

  getMovements(processId: number, page = 1, limit = 10): Observable<ProcessHistory[]> {
    const url = `${this.apiUrl}/${processId}/historicos?page=${page}&limit=${limit}`;
    return this.http.get<ProcessHistory[]>(url).pipe(catchError((err) => this.handleError(err)));
  }

  createMovement(processId: number, movement: { descricao: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${processId}/historicos`, movement).pipe(
      tap(() => this.toastService.show('Movimento criado com sucesso.', 'success')),
      catchError((err) => this.handleError(err))
    );
  }

  updateMovement(
    processId: number,
    movementId: number,
    movement: { descricao: string }
  ): Observable<any> {
    return this.http
      .put<any>(`${this.apiUrl}/${processId}/historicos/${movementId}`, movement)
      .pipe(
        tap(() => this.toastService.show('Movimento atualizado com sucesso.', 'success')),
        catchError((err) => this.handleError(err))
      );
  }

  deleteMovement(processId: number, movementId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${processId}/historicos/${movementId}`).pipe(
      tap(() => this.toastService.show('Movimento excluído com sucesso.', 'success')),
      catchError((err) => this.handleError(err))
    );
  }

  private handleError(err: any) {
    const message = err?.error?.Message || err?.message || 'Ocorreu um erro na requisição.';
    this.toastService.showError(message);
    return throwError(() => err);
  }
}
