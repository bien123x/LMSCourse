import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CourseDto, CourseFiltersDto } from '../models/course-model';
import { QueryCourseEnrolledDto } from '../models/query-model';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Courses';

  getCourseFilters(): Observable<CourseFiltersDto> {
    return this.http.get<CourseFiltersDto>(`${this.apiUrl}/filters`);
  }
  getCourses(userId: number): Observable<CourseDto[]> {
    return this.http.get<CourseDto[]>(`${this.apiUrl}/all/${userId}`);
  }
  getCoursesWithFilters(filters: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/all-with-filter`, filters);
  }
  getCourseById(id: number): Observable<CourseDto> {
    return this.http.get<CourseDto>(`${this.apiUrl}/${id}`);
  }

  getCoursesEnrolled(query: QueryCourseEnrolledDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/enrolled`, query);
  }

  // get-courses-by-listId
  getCoursesByListId(courseIds: number[]): Observable<CourseDto[]> {
    return this.http.post<CourseDto[]>(`${this.apiUrl}/get-courses-by-listId`, courseIds);
  }

  getRemainingCapacity(courseId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/remaining-apacity/${courseId}`);
  }
}
