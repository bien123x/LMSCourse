import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CartItemComponent } from './cart-item/cart-item.component';
import { Router } from '@angular/router';
import { CartService } from '../../../../core/services/cart.service';
import { CourseDto } from '../../../../core/models/course-model';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  imports: [ButtonModule, CartItemComponent, CurrencyPipe],
})
export class CartComponent implements OnInit {
  private cartService = inject(CartService);
  private router = inject(Router);
  cart = signal<CourseDto[]>([]);

  ngOnInit(): void {
    this.cart.set(this.cartService.cart());
  }
  // totalItems =

  totalItems() {
    return this.cartService.totalItems();
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

  onDeleteItem(event: any) {
    this.cartService.removeItem(event);
    this.cart.set(this.cartService.cart());
  }
  clearCart() {
    this.cartService.clearCart();
    this.cart.set(this.cartService.cart());
  }

  continueShopping() {
    this.router.navigate(['/student/courses']);
  }

  checkoutCart() {
    console.log('checkout');
    this.router.navigate(['/student/checkout']);
  }
}
