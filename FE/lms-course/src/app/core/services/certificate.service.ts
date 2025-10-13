import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CertificateDto, CertificateViewDto } from '../models/certificate-model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CertificateService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Certificates';

  generateCertificate(dto: CertificateDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/generate-certificate`, dto);
  }

  getByUserId(userId: number): Observable<CertificateViewDto[]> {
    return this.http.get<CertificateViewDto[]>(`${this.apiUrl}/by-user/${userId}`);
  }
}
