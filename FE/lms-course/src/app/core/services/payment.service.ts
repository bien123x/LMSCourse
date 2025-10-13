import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { PaymentDto } from '../models/payment-model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Payment';

  createOrder(courseDtos: PaymentDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/create`, courseDtos);
  }
  getStatus(appTransId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/query-status`, JSON.stringify(appTransId), {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  getRecentInvoices(): Observable<PaymentDto[]> {
    return this.http.get<PaymentDto[]>(`${this.apiUrl}/get-recent-invoices`);
  }
}
