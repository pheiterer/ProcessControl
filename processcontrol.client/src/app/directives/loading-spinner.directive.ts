import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appLoadingSpinner]',
  standalone: true,
})
export class LoadingSpinnerDirective {
  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef
  ) {}

  @Input()
  set appLoadingSpinner(isLoading: boolean) {
    if (isLoading) {
      this.viewContainer.clear();
      // You can customize the spinner HTML here
      this.viewContainer.createEmbeddedView(this.templateRef);
      const spinnerElement = document.createElement('div');
      spinnerElement.className = 'text-center py-4';
      spinnerElement.innerHTML = `
        <div class="spinner-border" role="status">
          <span class="visually-hidden">Carregando...</span>
        </div>
      `;
      this.viewContainer.element.nativeElement.parentNode.insertBefore(
        spinnerElement,
        this.viewContainer.element.nativeElement
      );
    } else {
      this.viewContainer.clear();
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }
}
