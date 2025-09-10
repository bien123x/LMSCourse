import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Role, RoleDto, ViewRolesDto } from '../models/role-model';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private apiUrl = 'https://localhost:7202/Role';
  private http = inject(HttpClient);

  getRoles(): Observable<ViewRolesDto[]> {
    return this.http.get<ViewRolesDto[]>(`${this.apiUrl}/view-roles-dto`);
  }

  addRole(roleDto: RoleDto): Observable<ViewRolesDto> {
    return this.http.post<ViewRolesDto>(`${this.apiUrl}/create-role`, roleDto);
  }

  editRole(roleId: number, roleDto: RoleDto): Observable<ViewRolesDto> {
    return this.http.put<ViewRolesDto>(`${this.apiUrl}/edit-role/${roleId}`, roleDto);
  }

  getPermissions(roleId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get-permissions/${roleId}`);
  }

  updatePermissions(roleId: number, permissions: string[]): Observable<string[]> {
    return this.http.put<string[]>(`${this.apiUrl}/update-permissions/${roleId}`, permissions, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  getCountUser(roleId: number): Observable<any> {
    return this.http.get<string>(`${this.apiUrl}/count-users-role/${roleId}`);
  }

  deleteRoleUnAssign(roleId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete-role-unassign/${roleId}`);
  }

  deleteRoleAssign(roleId: number, roleIdAssign: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete-role-assign/${roleId}/${roleIdAssign}`);
  }

  getRolesMinusRole(roleId: number): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.apiUrl}/roles-minus-role/${roleId}`);
  }
}
