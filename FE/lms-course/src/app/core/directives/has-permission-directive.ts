import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[appHasPermission]',
})
export class HasPermissionDirective {
  private requiredPermission: string = '';

  constructor(
    private authService: AuthService,
    private templateRef: TemplateRef<any>,
    private vcr: ViewContainerRef
  ) {}

  @Input()
  set appHasPermission(permission: string) {
    this.requiredPermission = permission;
    this.updateView();
  }

  private updateView() {
    if (this.authService.hasPermission(this.requiredPermission)) {
      this.vcr.createEmbeddedView(this.templateRef);
    } else {
      this.vcr.clear();
    }
  }
}
