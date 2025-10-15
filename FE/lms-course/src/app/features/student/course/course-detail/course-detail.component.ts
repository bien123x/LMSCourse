import { PERMISSION } from './../../../../core/models/constant';
import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AccordionModule } from 'primeng/accordion';
import { CurrencyPipe } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { CourseService } from '../../../../core/services/course.service';
import { CourseDto } from '../../../../core/models/course-model';
import { CartService } from '../../../../core/services/cart.service';
import { switchMap } from 'rxjs';
import { HasPermissionDirective } from "../../../../core/directives/has-permission-directive";
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
  imports: [AccordionModule, CurrencyPipe, ButtonModule, HasPermissionDirective, CardModule],
})
export class CourseDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  courseId: number = 0;
  course = signal<CourseDto | undefined>(undefined);
  private cartService = inject(CartService);
  private router = inject(Router);

  private cd = inject(ChangeDetectorRef);

  PERMISSION = PERMISSION;

  remainingCapacity: number = 0;

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.courseId = +params['id']; // the '+' converts the string to a number

      this.courseService
        .getRemainingCapacity(this.courseId)
        .pipe(
          switchMap((remainingCapacity) => {
            this.remainingCapacity = remainingCapacity;
            console.log(remainingCapacity);
            this.cd.detectChanges();
            return this.courseService.getCourseById(this.courseId);
          })
        )
        .subscribe((data: any) => {
          this.course.set(data.data);
        });
      // this.courseService.getCourseById(this.courseId)
      // .subscribe((data: any) => {
      //   this.course.set(data.data);
      // });
    });
  }

  get priceAfterDiscount() {
    return this.course()!.price - this.course()!.discountPrice;
  }

  addToCart() {
    this.cartService.addItem(this.course()!);
    this.router.navigate(['/student/cart']);
  }

  isInCart(): boolean {
    return this.cartService.isInCart(this.courseId);
  }
}
