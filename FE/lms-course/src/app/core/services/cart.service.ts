import { computed, Injectable, signal } from '@angular/core';
import { CourseDto } from '../models/course-model';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private cartKey = 'my_cart';

  cart = signal<CourseDto[]>(this.loadCart());

  // Computed signal (ví dụ đếm số item)
  totalItems = computed(() => this.cart().length);
  constructor() {
    this.cart.set(this.loadCart());
  }

  private loadCart(): CourseDto[] {
    const cart = typeof localStorage !== 'undefined' ? localStorage.getItem(this.cartKey) : null;
    return cart ? JSON.parse(cart) : [];
  }

  // private addCart()
  private saveCart(cart: CourseDto[]) {
    localStorage.setItem(this.cartKey, JSON.stringify(cart));
    this.cart.set(cart);
  }

  addItem(item: CourseDto) {
    const cart = [...this.cart(), item];
    this.saveCart(cart);
  }

  removeItem(courseId: number) {
    const cart = [...this.cart()];

    // Tìm vị trí item theo courseId
    const index = cart.findIndex((x) => x.courseId === courseId);

    if (index > -1) {
      cart.splice(index, 1);
      this.saveCart(cart);
    }
  }

  clearCart() {
    this.saveCart([]);
  }

  isInCart(courseId: number): boolean {
    return this.cart().some((item) => item.courseId === courseId);
  }
}
