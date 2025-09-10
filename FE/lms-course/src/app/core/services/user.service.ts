import { EditUserDto, UserDto, ViewUserDto } from './../models/user-model';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://localhost:7202/User';
  private http = inject(HttpClient);

  getViewUsers(): Observable<ViewUserDto[]> {
    return this.http.get<ViewUserDto[]>(`${this.apiUrl}/all-view-user`);
  }

  addUser(userDto: UserDto): Observable<ViewUserDto> {
    return this.http.post<ViewUserDto>(`${this.apiUrl}/add-user`, userDto);
  }

  editUser(userId: number, editUserDto: EditUserDto): Observable<ViewUserDto> {
    return this.http.put<ViewUserDto>(`${this.apiUrl}/edit-user/${userId}`, editUserDto);
  }

  getRolesName(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/roles-name`);
  }
}
