import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PasswordPolicy } from '../models/settings-model';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  private apiUrl = 'https://localhost:7202/Settings';

  private http = inject(HttpClient);

  getPasswordPolicy(): Observable<PasswordPolicy> {
    return this.http.get<PasswordPolicy>(`${this.apiUrl}`);
  }

  updatePasswordPolicy(passwordPolicy: PasswordPolicy): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}`, passwordPolicy);
  }

  validatePassword(password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, JSON.stringify(password), {
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
