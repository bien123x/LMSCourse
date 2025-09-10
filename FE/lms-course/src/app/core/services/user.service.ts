import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ViewUserDto } from '../models/user-model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://localhost:7202/User';
  private http = inject(HttpClient);

  getViewUsers(): Observable<ViewUserDto[]> {
    return this.http.get<ViewUserDto[]>(`${this.apiUrl}/all-view-user`);
  }
}
