import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal, WritableSignal } from '@angular/core';
import { DecodedToken, LoginDto, RefreshRequestDto, RegisterDto } from '../models/auth-model';
import { Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { ViewUserDto } from '../models/user-model';
import { MenuItem } from 'primeng/api';
import { CodePermission } from '../models/constant';
import { Router } from '@angular/router';
import { CartService } from './cart.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7202/Auth';
  private http = inject(HttpClient);
  private router = inject(Router);

  private accessTokenKey = 'access_token';
  private refreshTokenKey = 'refresh_token';
  private rolesKey = 'roles';
  private permissionsKey = 'permissions';

  private cartService = inject(CartService);

  menuItems = signal<MenuItem[]>([]);
  private adminMenu = computed<MenuItem[]>(() => [
    {
      label: 'Trang chủ',
      icon: 'pi pi-home',
      command: () => this.router.navigate(['/home']),
    },
    {
      visible: this.hasPermission(CodePermission.System),
      label: 'Quản trị',
      icon: 'pi pi-cog',
      items: [
        {
          visible: this.hasPermission(CodePermission.UserManagement),
          label: 'Quản lý tài khoản',
          icon: 'pi pi-users',
          items: [
            {
              visible: this.hasPermission(CodePermission.Roles),
              label: 'Quyền',
              icon: 'pi pi-lock',
              command: () => this.router.navigate(['/admin/identity/roles']),
            },
            {
              visible: this.hasPermission(CodePermission.Users),
              label: 'Người dùng',
              icon: 'pi pi-user',
              command: () => this.router.navigate(['/admin/identity/users']),
            },
          ],
        },
        {
          visible: this.hasPermission(CodePermission.AuditLogs),
          label: 'Nhật ký',
          icon: 'pi pi-book',
          command: () => this.router.navigate(['/admin/audit-logs']),
        },
        {
          visible: this.hasPermission(CodePermission.Settings),
          label: 'Cài đặt',
          icon: 'pi pi-sliders-h',
          command: () => this.router.navigate(['/admin/settings']),
        },
      ],
    },
  ]);

  private teacherMenu = computed<MenuItem[]>(() => []);

  private studentMenu = computed<MenuItem[]>(() => [
    {
      visible: this.hasPermission(CodePermission.ViewUsers),

      label: 'Dashboard',
      icon: 'pi pi-chart-line',
      command: () => this.router.navigate(['/dashboard']),
    },
    {
      label: 'Dashboard',
      icon: 'pi pi-chart-line',
      command: () => this.router.navigate(['/student/dashboard']),
    },
    {
      label: 'Khoá học đã tham gia',
      icon: 'pi pi-chart-line',
      command: () => this.router.navigate(['/student/courses-enrolled']),
    },
    {
      label: 'Khóa học',
      icon: 'pi pi-book',
      command: () => this.router.navigate(['student/courses']),
    },
    {
      label: 'Chứng chỉ',
      icon: 'pi pi-wallet',
      command: () => this.router.navigate(['student/certificates']),
    },
  ]);

  // topbarItems = signal<>

  // --- Reactive state ---
  private loggedIn = signal<boolean>(this.hasValidToken());
  isLoggedIn = computed(() => this.loggedIn());

  private rolesSignal = signal<string[]>(this.loadArray(this.rolesKey));
  private permissionsSignal = signal<string[]>(this.loadArray(this.permissionsKey));

  constructor() {
    const token = typeof localStorage !== 'undefined' ? localStorage.getItem('access_token') : null;

    if (token) this.decodeAndSetClaims(token);

    this.buildMenu();
  }

  // --- Helpers ---
  private hasValidToken(): boolean {
    const token = typeof localStorage !== 'undefined' ? localStorage.getItem('access_token') : null;
    if (token) return true;
    return false;
  }

  private loadArray(key: string): string[] {
    const data = typeof localStorage !== 'undefined' ? localStorage.getItem(key) : null;
    return data ? JSON.parse(data) : [];
  }

  private saveArray(key: string, arr: string[]) {
    localStorage.setItem(key, JSON.stringify(arr));
  }

  private decodeAndSetClaims(token: string | null) {
    if (!token || typeof token !== 'string') {
      console.error('Token không hợp lệ:', token);
      return;
    }

    try {
      const decoded = jwtDecode<any>(token);

      const roleClaim = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      const roles = Array.isArray(roleClaim) ? roleClaim : roleClaim ? [roleClaim] : [];
      this.rolesSignal.set(roles);
      this.saveArray(this.rolesKey, roles);

      const permissionClaim = decoded.Permission;
      const permissions = Array.isArray(permissionClaim)
        ? permissionClaim
        : permissionClaim
        ? [permissionClaim]
        : [];
      this.permissionsSignal.set(permissions);
      this.saveArray(this.permissionsKey, permissions);
    } catch (err) {
      console.error('Decode JWT thất bại:', err);
    }
  }

  private buildMenu() {
    // tuỳ role mà set menu
    if (this.rolesSignal().includes('Admin')) {
      this.menuItems.set(this.adminMenu());
    } else if (this.rolesSignal().includes('Teacher')) {
      this.menuItems.set(this.teacherMenu());
    } else if (this.rolesSignal().includes('Student')) {
      this.menuItems.set(this.studentMenu());
    } else {
      this.menuItems.set([]);
    }
  }

  // --- Public API ---
  getRoles() {
    return this.rolesSignal();
  }

  getPermissions() {
    return this.permissionsSignal();
  }

  hasPermission(permission: string): boolean {
    return this.permissionsSignal().includes(permission);
  }

  hasRole(role: string): boolean {
    return this.rolesSignal().includes(role);
  }

  me(): Observable<ViewUserDto> {
    return this.http.get<ViewUserDto>(`${this.apiUrl}/me`);
  }

  login(loginDto: LoginDto): Observable<any> {
    return this.http.post<LoginDto>(`${this.apiUrl}/login`, loginDto).pipe(
      tap((res: any) => {
        if (res.accessToken) {
          const token = res.accessToken;
          localStorage.setItem(this.accessTokenKey, res.accessToken);
          localStorage.setItem(this.refreshTokenKey, res.refreshToken);

          this.loggedIn.set(true);
          this.decodeAndSetClaims(token);

          this.buildMenu();
        }
      })
    );
  }

  refreshToken(refreshToken: RefreshRequestDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/refresh`, refreshToken).pipe(
      tap((res: any) => {
        const token = res.accessToken.result;
        localStorage.setItem(this.accessTokenKey, token);
        localStorage.setItem(this.refreshTokenKey, res.refreshToken);

        this.decodeAndSetClaims(token);
      })
    );
  }

  register(registerDto: RegisterDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, registerDto);
  }

  logout() {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.rolesKey);
    localStorage.removeItem(this.permissionsKey);

    this.loggedIn.set(false);
    localStorage.removeItem(this.rolesKey);
    localStorage.removeItem(this.permissionsKey);
    localStorage.setItem('my_cart', JSON.stringify([]));
    this.cartService.clearCart();
  }

  loadCurrentUser(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/me`);
  }
}
