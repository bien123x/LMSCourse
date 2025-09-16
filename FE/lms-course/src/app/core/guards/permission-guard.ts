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
export class PermissionGuard implements CanActivate {
  private authService = inject(AuthService);
  private router = inject(Router);

  canActivate(route: ActivatedRouteSnapshot): boolean {
    let expectedPermissions = route.data['permissions'] || [];
    const match: 'all' | 'any' = route.data['match'] || 'all'; // mặc định là cần tất cả
    const userPermissions = this.authService.getPermissions();

    // ép về mảng nếu chỉ truyền string
    if (!Array.isArray(expectedPermissions)) {
      expectedPermissions = [expectedPermissions];
    }

    let isAllowed = false;

    if (match === 'all') {
      isAllowed = expectedPermissions.every((p: string) => userPermissions.includes(p));
    } else {
      isAllowed = expectedPermissions.some((p: string) => userPermissions.includes(p));
    }

    if (isAllowed) return true;

    this.router.navigate(['/home']);
    return false;
  }
}
