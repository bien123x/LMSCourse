import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal, WritableSignal } from '@angular/core';
import { LoginDto, RefreshRequestDto, RegisterDto } from '../models/auth-model';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7202/Auth';
  private http = inject(HttpClient);

  private accessTokenKey = 'access_token';
  private refreshTokenKey = 'refresh_token';

  // private user = signal<any>(this.getUser());
  // currentUser = computed(() => this.user());

  private loggedIn = signal<boolean>(this.hasValidToken());
  isLoggedIn = computed(() => this.loggedIn());

  private hasValidToken(): boolean {
    const token = typeof localStorage !== 'undefined' ? localStorage.getItem('access_token') : null;
    if (token) return true;
    return false;
  }

  login(loginDto: LoginDto): Observable<any> {
    return this.http.post<LoginDto>(`${this.apiUrl}/login`, loginDto).pipe(
      tap((res: any) => {
        localStorage.setItem(this.accessTokenKey, res.accessToken.result);
        localStorage.setItem(this.refreshTokenKey, res.refreshToken);
        this.loggedIn.set(true);
      })
    );
  }

  refreshToken(refreshToken: RefreshRequestDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/refresh`, refreshToken);
  }

  register(registerDto: RegisterDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, registerDto);
  }

  logout() {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    this.loggedIn.set(false);
  }

  loadCurrentUser(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/me`);
  }
}
