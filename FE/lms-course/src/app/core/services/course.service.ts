import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import e from 'express';
import { Observable } from 'rxjs';
import { CourseDto, CourseFiltersDto } from '../models/course-model';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Courses';

  getCourseFilters(): Observable<CourseFiltersDto> {
    return this.http.get<CourseFiltersDto>(`${this.apiUrl}/filters`);
  }
  getCourses(): Observable<CourseDto[]> {
    return this.http.get<CourseDto[]>(this.apiUrl);
  }
  getCoursesWithFilters(filters: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/all-with-filter`, filters);
  }
  getCourseById(id: number): Observable<CourseDto> {
    return this.http.get<CourseDto>(`${this.apiUrl}/${id}`);
  }
}
