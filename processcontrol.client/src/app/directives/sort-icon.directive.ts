import { Directive, Input, ElementRef, Renderer2, OnChanges, SimpleChanges } from '@angular/core';

@Directive({
  selector: '[appSortIcon]',
  standalone: true,
})
export class SortIconDirective implements OnChanges {
  @Input() appSortIcon!: string; // The column name for this header
  @Input() currentSortColumn: string | null = null;
  @Input() currentSortDirection: 'asc' | 'desc' | '' = '';

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentSortColumn'] || changes['currentSortDirection']) {
      this.updateSortIcon();
    }
  }

  private updateSortIcon(): void {
    // Clear existing icons
    const existingIcons = this.el.nativeElement.querySelectorAll('i');
    existingIcons.forEach((icon: HTMLElement) =>
      this.renderer.removeChild(this.el.nativeElement, icon)
    );

    const icon = this.renderer.createElement('i');
    this.renderer.addClass(icon, 'bi');

    if (this.appSortIcon === this.currentSortColumn) {
      if (this.currentSortDirection === 'asc') {
        this.renderer.addClass(icon, 'bi-caret-up-fill');
      } else if (this.currentSortDirection === 'desc') {
        this.renderer.addClass(icon, 'bi-caret-down-fill');
      }
    } else {
      this.renderer.addClass(icon, 'bi-sort-down');
    }
    this.renderer.appendChild(this.el.nativeElement, icon);
  }
}
