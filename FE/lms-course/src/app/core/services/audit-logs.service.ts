import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QueryDto } from '../models/query-model';
import { PagedResult } from '../models/page-model';
import { AuditlogsDto } from '../models/audit-logo-model';

@Injectable({
  providedIn: 'root',
})
export class AuditLogsService {
  private apiUrl = 'https://localhost:7202/AuditLogs';
  private http = inject(HttpClient);

  getAllAuditLogs(): Observable<AuditlogsDto[]> {
    return this.http.get<AuditlogsDto[]>(`${this.apiUrl}`);
  }

  getAllAuditLogByQuery(query: QueryDto): Observable<PagedResult<AuditlogsDto>> {
    return this.http.post<PagedResult<AuditlogsDto>>(`${this.apiUrl}/audit-logs`, query);
  }
}
