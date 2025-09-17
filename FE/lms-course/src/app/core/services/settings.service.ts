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

  getIdentitySetting(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/identity`);
  }

  updateIdentitySetting(identitySettingDto: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/identity`, identitySettingDto);
  }

  validatePassword(password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, JSON.stringify(password), {
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
