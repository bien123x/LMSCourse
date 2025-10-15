import {
  ChangeDetectorRef,
  Directive,
  Injectable,
  Input,
  TemplateRef,
  ViewContainerRef,
  inject,
} from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[appCurrentRole]',
})
export class CurrentRoleDirective {
  private currentRole: string = '';
  constructor(
    private authService: AuthService,
    private templateRef: TemplateRef<any>,
    private vcr: ViewContainerRef
  ) {}

  @Input()
  set appCurrentRole(currentRole: string) {
    this.currentRole = currentRole;
    this.updateView();
  }

  private updateView() {
    if (this.authService.getCurrentRole() == this.currentRole) {
      this.vcr.clear();
      this.vcr.createEmbeddedView(this.templateRef);
    } else {
      this.vcr.clear();
    }
  }
}
