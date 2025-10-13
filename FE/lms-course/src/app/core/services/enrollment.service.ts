import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardEnrollmentCourseDto, EnrollmentDto } from '../models/course-model';

@Injectable({
  providedIn: 'root',
})
export class EnrollmentService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Enrollments';

  getEnrollmentById(courseId: number): Observable<EnrollmentDto> {
    return this.http.get<EnrollmentDto>(`${this.apiUrl}/${courseId}`);
  }
  getDashboardEnrolledCoure(): Observable<DashboardEnrollmentCourseDto> {
    return this.http.get<DashboardEnrollmentCourseDto>(
      `${this.apiUrl}/dashboard-enrollment-course`
    );
  }
  updateStatusEnrolledCourse(courseId: number): Observable<EnrollmentDto> {
    return this.http.get<EnrollmentDto>(`${this.apiUrl}/update-status/${courseId}`);
  }

  isExistCertificate(enrollmentId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/is-exist-certificate/${enrollmentId}`);
  }
}
