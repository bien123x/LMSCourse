import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';
import { PaymentService } from '../../../core/services/payment.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-payment-result',
  templateUrl: './payment-result.component.html',
})
export class PaymentResultComponent implements OnInit {
  paymentData: any = {};
  private cartService = inject(CartService);
  private paymentService = inject(PaymentService);
  private msgService = inject(MessageService);
  status = signal<number>(0);

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    // Lấy query params
    this.route.queryParams.subscribe((params) => {
      this.paymentData.amount = params['amount'];
      this.paymentData.appid = params['appid'];
      this.paymentData.apptransid = params['apptransid'];
      this.paymentData.bankcode = params['bankcode'];
      this.paymentData.checksum = params['checksum'];
      this.paymentData.discountamount = params['discountamount'];
      this.paymentData.pmcid = params['pmcid'];
      this.paymentData.status = params['status'];

      // Xử lý tiếp, ví dụ kiểm tra status
      this.paymentService.getStatus(this.paymentData.apptransid).subscribe((res) => {
        this.status.set(res.return_code);
        if (this.status() !== 1) {
          console.log('Giao dịch thất bại hoặc lỗi.');
          this.msgService.add({
            severity: 'error',
            summary: 'Thất bại',
            detail: 'Thanh toán thất bại',
          });
        } else if (this.status() === 1) {
          console.log('Thanh toán thành công.');
          this.cartService.clearCart();

          this.msgService.add({
            severity: 'success',
            summary: 'Thành công',
            detail: 'Thanh toán thành công',
          });
        }
      });
    });
  }
}
