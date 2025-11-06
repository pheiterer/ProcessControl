import { Directive, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { ProcessModel } from '../models/process.model';

export type SortDirection = 'asc' | 'desc' | '';
const rotate: Record<string, SortDirection> = { asc: 'desc', desc: '', '': 'asc' };

export interface SortEvent {
  column: string;
  direction: SortDirection;
}

@Directive({
  selector: '[appSortableTable]',
  standalone: true,
})
export class SortableTableDirective {
  @Input() sortableData: ProcessModel[] = [];
  @Output() sort = new EventEmitter<SortEvent>();

  @Input() sortColumn: string | null = null;
  @Input() sortDirection: SortDirection = '';

  @HostListener('click', ['$event.target'])
  onSort(target: EventTarget | null) {
    const htmlTarget = target as HTMLElement;
    const column = htmlTarget?.getAttribute('sortKey');
    if (column) {
      this.sortColumn = column;
      this.sortDirection = rotate[this.sortDirection];
      this.sort.emit({ column: this.sortColumn, direction: this.sortDirection });
    }
  }
}
