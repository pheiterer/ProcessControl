import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ToastMessage {
  id: string;
  text: string;
  type?: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark';
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private messages$ = new BehaviorSubject<ToastMessage[]>([]);

  readonly messages = this.messages$.asObservable();

  show(text: string, type: ToastMessage['type'] = 'danger', timeout = 5000): void {
    const msg: ToastMessage = { id: `${Date.now()}-${Math.random()}`, text, type };
    this.messages$.next([...this.messages$.getValue(), msg]);
    if (timeout > 0) {
      setTimeout(() => this.remove(msg.id), timeout);
    }
  }

  showError(text: string, timeout = 5000): void {
    this.show(text, 'danger', timeout);
  }

  remove(id: string): void {
    this.messages$.next(this.messages$.getValue().filter((m) => m.id !== id));
  }
}
