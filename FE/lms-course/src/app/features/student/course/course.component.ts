import { Component, inject, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CourseFiltersComponent } from './course-filters/course-filters.component';
import { CourseListComponent } from './course-list/course-list.component';
import { Router } from '@angular/router';
import { CourseDto, CourseFiltersDto } from '../../../core/models/course-model';
import { CourseFilterRequest, QueryCourseDto } from '../../../core/models/query-model';
import { CourseService } from '../../../core/services/course.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css'],
  imports: [ButtonModule, CourseFiltersComponent, CourseListComponent],
})
export class CourseComponent implements OnInit {
  private courseService = inject(CourseService);
  private authService = inject(AuthService);
  private router = inject(Router);
  rows = signal<number>(5);
  pageNumber = signal<number>(1);
  totalRecodes = signal<number>(0);
  courses = signal<CourseDto[]>([]);
  sortChange = signal<{ field: string; order: number }>({ field: '', order: 0 });
  courseFilters = signal<CourseFiltersDto | undefined>(undefined);

  selected: CourseFilterRequest = {
    teacherIds: [],
    categoryIds: [],
    levelIds: [],
    languageIds: [],
  };
  ngOnInit() {
    this.courseService.getCourseFilters().subscribe((filters) => {
      this.courseFilters.set(filters);
    });
    this.authService.me().subscribe((res) => {
      this.courseService.getCourses(res.userId).subscribe((courses) => {
        this.courses.set(courses);
        console.log(this.courses());
      });
    });
  }
  setSortChange(event: any) {
    console.log('Emit event change sort:', event);
    this.sortChange.set(event);
    this.loadCourses();
  }

  onSelectedFilters(event: any) {
    this.selected = event;
    this.pageNumber.set(1);
    this.loadCourses();
  }

  loadCourses() {
    const queryCourseDto: QueryCourseDto = {
      pageNumber: this.pageNumber(),
      pageSize: this.rows(),
      sortField: this.sortChange().field,
      sortOrder: this.sortChange().order,
      courseFilterRequest: this.selected,
    };
    console.log('Query:', queryCourseDto);
    this.courseService.getCoursesWithFilters(queryCourseDto).subscribe((res) => {
      this.courses.set(res.items);
      this.totalRecodes.set(res.totalCount);
      console.log(this.courses());
    });
  }

  setPageNumber(event: any) {
    this.pageNumber.set(event);
  }

  viewDetail(event: any) {
    this.router.navigate(['student/courses/detail', event]);
  }
}
