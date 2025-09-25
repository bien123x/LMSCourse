import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseDto } from '../../../core/models/course-model';
import { CourseService } from '../../../core/services/course.service';
import { AccordionModule } from 'primeng/accordion';
import { CurrencyPipe } from '@angular/common';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
  imports: [AccordionModule, CurrencyPipe, ButtonModule],
})
export class CourseDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  courseId: number = 0;
  course = signal<CourseDto | undefined>(undefined);

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.courseId = +params['id']; // the '+' converts the string to a number

      this.courseService.getCourseById(this.courseId).subscribe((data: any) => {
        this.course.set(data.data);
        console.log('Data:', data.data);
        console.log('Course:', this.course());
      });
    });
  }

  get priceAfterDiscount() {
    return this.course()!.price - this.course()!.discountPrice;
  }
}
