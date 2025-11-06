import { Directive, ElementRef, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Directive({
  selector: '[appInfiniteScroll]',
  standalone: true,
})
export class InfiniteScrollDirective {
  @Input() infiniteScrollDistance = 300; // Distance from bottom in pixels to trigger load
  @Input() infiniteScrollDisabled = false;
  @Output() appInfiniteScroll = new EventEmitter<void>();

  constructor(private el: ElementRef) {}

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    if (this.infiniteScrollDisabled) {
      return;
    }

    const scrollPosition = window.innerHeight + window.scrollY;
    const scrollHeight = document.documentElement.scrollHeight;

    if (scrollPosition >= scrollHeight - this.infiniteScrollDistance) {
      this.appInfiniteScroll.emit();
    }
  }
}
