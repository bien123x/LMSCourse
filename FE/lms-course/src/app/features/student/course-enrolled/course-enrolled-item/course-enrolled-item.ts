import { Component, inject, input, model } from '@angular/core';
import { CourseDto } from '../../../../core/models/course-model';
import { ButtonModule } from 'primeng/button';
import { Avatar } from 'primeng/avatar';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-course-enrolled-item',
  templateUrl: './course-enrolled-item.html',
  imports: [Avatar, ButtonModule, CardModule, CurrencyPipe],
})
export class CourseEnrolledItemComponent {
  private router = inject(Router);
  course = model<CourseDto>();

  watch(courseId: number) {
    this.router.navigate(['/student/courses-enrolled', courseId]);
  }
}
