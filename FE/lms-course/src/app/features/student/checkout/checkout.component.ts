import { Component, inject, OnInit, signal } from '@angular/core';
import { CartService } from '../../../core/services/cart.service';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FormsModule } from '@angular/forms';
import { CourseDto } from '../../../core/models/course-model';
import { CartItemComponent } from '../course/cart/cart-item/cart-item.component';
import { ButtonModule } from 'primeng/button';
import { PaymentService } from '../../../core/services/payment.service';
import { PaymentDto } from '../../../core/models/payment-model';
import { AuthService } from '../../../core/services/auth.service';
import { CardModule } from 'primeng/card';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  imports: [
    SelectButtonModule,
    FormsModule,
    CartItemComponent,
    ButtonModule,
    CardModule,
    CurrencyPipe,
  ],
})
export class CheckoutComponent implements OnInit {
  private cartService = inject(CartService);
  private paymentService = inject(PaymentService);
  private authService = inject(AuthService);
  paymentOptions: any[] = [
    { label: 'Zalopay QR Code', value: 'zalopayapp' },
    { label: '...', value: '...' },
  ];
  paymentMethod = 'zalopayapp';

  cart = signal<CourseDto[]>([]);

  paymentDto: PaymentDto = {
    userId: 0,
    amount: 0,
    courseDtos: [],
    method: '',
    appTransId: '',
    status: 'pending',
  };

  appTransId: string = '';

  ngOnInit(): void {
    this.cart.set(this.cartService.cart());

    this.authService.me().subscribe((res) => (this.paymentDto.userId = res.userId));
  }

  get totalPrice() {
    return this.cart().reduce((acc, val) => acc + this.price(val), 0);
  }

  price(item: CourseDto) {
    if (item.isFree) return 0;
    if (item.hasDiscount) {
      return item.price! - item.discountPrice!;
    }
    return item.price;
  }

  createPayment() {
    this.paymentDto.method = this.paymentMethod;
    this.paymentDto.amount = this.totalPrice;
    this.paymentDto.courseDtos = this.cart();

    this.paymentService.createOrder(this.paymentDto).subscribe((res) => {
      console.log(res);
      if (res.order_url) {
        this.appTransId = res.app_trans_id; // lưu lại để check status
        window.location.href = res.order_url; // Redirect sang ZaloPay
      }
    });
  }
}
