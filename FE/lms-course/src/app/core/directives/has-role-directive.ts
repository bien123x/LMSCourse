import { Directive, Injectable, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[appHasRole]',
})
export class HasRoleDirective {
  private requiredRole: string = '';
  constructor(
    private authService: AuthService,
    private templateRef: TemplateRef<any>,
    private vcr: ViewContainerRef
  ) {}

  @Input()
  set appHasRole(role: string) {
    this.requiredRole = role;
    this.updateView();
  }

  private updateView() {
    if (this.authService.hasRole(this.requiredRole)) {
      this.vcr.createEmbeddedView(this.templateRef);
    } else {
      this.vcr.clear();
    }
  }
}
