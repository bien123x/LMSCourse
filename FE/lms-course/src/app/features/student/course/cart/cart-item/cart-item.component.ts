import { Component, input, model, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Avatar } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { CourseDto } from '../../../../../core/models/course-model';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  imports: [RouterLink, Avatar, ButtonModule, CurrencyPipe],
})
export class CartItemComponent {
  item = model<CourseDto | undefined>(undefined);
  visibleDelete = input<boolean>(true);
  deleteCourseId = output<number>();

  deleteItem() {
    this.deleteCourseId.emit(this.item()?.courseId!);
  }

  get price() {
    if (this.item()?.isFree) return 0;
    if (this.item()?.hasDiscount) {
      return this.item()?.price! - this.item()?.discountPrice!;
    }
    return this.item()?.price;
  }
}
