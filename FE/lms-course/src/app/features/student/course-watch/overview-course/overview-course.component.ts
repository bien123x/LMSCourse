import { Component, input, model, OnInit } from '@angular/core';
import { CourseEnrolledDto } from '../../../../core/models/course-model';

@Component({
  selector: 'app-overview-course',
  templateUrl: './overview-course.component.html',
})
export class OverviewCourseComponent implements OnInit {
  course = model<CourseEnrolledDto>();
  ngOnInit(): void {
    console.log('dsadsa', this.course());
  }
}
