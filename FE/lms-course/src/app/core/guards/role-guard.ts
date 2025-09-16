import { inject, Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  private authService = inject(AuthService);
  private router = inject(Router);

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const expectedRoles: string[] = route.data['roles'];
    const roles: string[] = this.authService.getRoles();

    // Nếu chỉ yêu cầu 1 role → check includes
    if (expectedRoles.length === 1) {
      const hasRole = roles.includes(expectedRoles[0]);
      if (hasRole) return true;
    }
    // Nếu yêu cầu nhiều role → check phải có tất cả
    else {
      const hasAllRoles = expectedRoles.every((role) => roles.includes(role));
      if (hasAllRoles) return true;
    }

    this.router.navigate(['/home']);
    return false;
  }
}
