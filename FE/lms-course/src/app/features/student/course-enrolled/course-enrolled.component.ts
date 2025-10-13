import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { CourseService } from '../../../core/services/course.service';
import { QueryCourseEnrolledDto } from '../../../core/models/query-model';
import { CourseDto } from '../../../core/models/course-model';
import { DataViewModule } from 'primeng/dataview';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FormsModule } from '@angular/forms';
import { Avatar } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { CourseEnrolledItemComponent } from './course-enrolled-item/course-enrolled-item';
import { first } from 'rxjs';

@Component({
  selector: 'app-course-enrolled',
  templateUrl: './course-enrolled.component.html',
  styleUrl: './course-enrolled.component.css',
  imports: [DataViewModule, SelectButtonModule, FormsModule, CourseEnrolledItemComponent],
})
export class CourseEnrolledComponent implements OnInit {
  private courseService = inject(CourseService);
  private cd = inject(ChangeDetectorRef);
  pageNumber = signal<number>(1);
  pageSize = signal<number>(5);
  totalRecode = signal<number>(0);
  courses = signal<CourseDto[]>([]);
  status: string = 'Active';
  filters = signal<{ value: string; label: string }[]>([]);
  ngOnInit(): void {
    this.filters.set([
      { value: 'Active', label: 'Kích hoạt' },
      // { value: 'Cancelled', label: 'Đã huỷ' },
      { value: 'Completed', label: 'Đã hoàn thành' },
    ]);

    this.loadCoursesEnrolled({ first: 0, rows: this.pageSize() });

    this.cd.markForCheck();
  }

  loadCoursesEnrolled(event: any) {
    this.pageNumber.set(event?.first != null ? event.first / event.rows + 1 : 1);
    console.log(this.status);
    const query: QueryCourseEnrolledDto = {
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
      sortOrder: 1,
      sortField: 'createdAt',
      status: this.status,
    };
    this.courseService.getCoursesEnrolled(query).subscribe((res) => {
      this.courses.set(res.items);
      this.totalRecode.set(res.totalCount);
      console.log(res);
    });
  }

  selectStatus(event: any) {
    this.loadCoursesEnrolled({ first: 0, rows: this.pageSize() });
  }
}
