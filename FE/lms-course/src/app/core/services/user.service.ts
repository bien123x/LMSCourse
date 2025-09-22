import {
  ChangePasswordDto,
  EditUserDto,
  LockEndTimeDto,
  ResetPasswordDto,
  UserDto,
  UserPermissionsDto,
  ViewUserDto,
} from './../models/user-model';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/page-model';
import { QueryDto } from '../models/query-model';

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

  getViewUserByID(userId: number): Observable<ViewUserDto> {
    return this.http.get<ViewUserDto>(`${this.apiUrl}/view-user/${userId}`);
  }

  getUserPermissions(userId: number): Observable<UserPermissionsDto> {
    return this.http.get<UserPermissionsDto>(`${this.apiUrl}/permissions-name/${userId}`);
  }

  updateUserPermissions(userId: number, permissionsName: string[]): Observable<string[]> {
    return this.http.put<string[]>(`${this.apiUrl}/user-permissions/${userId}`, permissionsName);
  }
  resetPassword(userId: number, resetPwd: ResetPasswordDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/reset-password/${userId}`, resetPwd);
  }
  deleteUser(userId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete-user/${userId}`);
  }

  getViewUsersPagination(query: QueryDto): Observable<PagedResult<ViewUserDto>> {
    return this.http.post<PagedResult<ViewUserDto>>(`${this.apiUrl}/users`, query);
  }

  changePassword(userId: number, changePasswordDto: ChangePasswordDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/change-password/${userId}`, changePasswordDto);
  }

  lockEndTimeApi(userId: number, dto: LockEndTimeDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/lock-user/${userId}`, dto);
  }
  unlockEndTimeApi(userId: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/unlock-user/${userId}`, null);
  }
}
