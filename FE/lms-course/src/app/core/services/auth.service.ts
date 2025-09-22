import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal, WritableSignal } from '@angular/core';
import { DecodedToken, LoginDto, RefreshRequestDto, RegisterDto } from '../models/auth-model';
import { Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7202/Auth';
  private http = inject(HttpClient);

  private accessTokenKey = 'access_token';
  private refreshTokenKey = 'refresh_token';
  private rolesKey = 'roles';
  private permissionsKey = 'permissions';

  // --- Reactive state ---
  private loggedIn = signal<boolean>(this.hasValidToken());
  isLoggedIn = computed(() => this.loggedIn());

  private rolesSignal = signal<string[]>(this.loadArray(this.rolesKey));
  private permissionsSignal = signal<string[]>(this.loadArray(this.permissionsKey));

  constructor() {
    const token = typeof localStorage !== 'undefined' ? localStorage.getItem('access_token') : null;

    if (token) this.decodeAndSetClaims(token);
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

  login(loginDto: LoginDto): Observable<any> {
    return this.http.post<LoginDto>(`${this.apiUrl}/login`, loginDto).pipe(
      tap((res: any) => {
        if (res.accessToken.result) {
          const token = res.accessToken.result;
          localStorage.setItem(this.accessTokenKey, res.accessToken.result);
          localStorage.setItem(this.refreshTokenKey, res.refreshToken);

          this.loggedIn.set(true);
          this.decodeAndSetClaims(token);
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
  }

  loadCurrentUser(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/me`);
  }
}
