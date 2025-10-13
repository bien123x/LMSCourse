import { Component, inject, OnInit, signal } from '@angular/core';
import { CardModule } from 'primeng/card';
import { EnrollmentService } from '../../../core/services/enrollment.service';
import { CourseDto, DashboardEnrollmentCourseDto } from '../../../core/models/course-model';
import { CourseEnrolledItemComponent } from '../course-enrolled/course-enrolled-item/course-enrolled-item';
import { CourseService } from '../../../core/services/course.service';
import { PaymentService } from '../../../core/services/payment.service';
import { PaymentDto } from '../../../core/models/payment-model';
import { InvoiceComponent } from './invoice/invoice.component';
import { UserQuizService } from '../../../core/services/user-quiz.service';
import { LatestQuizDto } from '../../../core/models/quiz-model';
import { LatestQuizComponent } from './latest-quiz/latest-quiz.component';

@Component({
  selector: 'app-dashboard-student',
  templateUrl: './dashboard-student.component.html',
  imports: [CardModule, CourseEnrolledItemComponent, InvoiceComponent, LatestQuizComponent],
})
export class DashboardStudentComponent implements OnInit {
  private enrollmentService = inject(EnrollmentService);
  private courseService = inject(CourseService);
  private paymentService = inject(PaymentService);
  private userQuizService = inject(UserQuizService);

  dashboardStudent = signal<DashboardEnrollmentCourseDto | null>(null);
  courses = signal<CourseDto[]>([]);
  paymentRecent = signal<PaymentDto[]>([]);
  latestQuizzes = signal<LatestQuizDto[]>([]);

  ngOnInit(): void {
    this.enrollmentService.getDashboardEnrolledCoure().subscribe((res) => {
      this.dashboardStudent.set(res);
      this.courseService
        .getCoursesByListId(res.recentEnrolledCourses.map((c) => c.courseId))
        .subscribe((courses) => {
          this.courses.set(courses);
        });
    });

    this.paymentService.getRecentInvoices().subscribe((payments) => {
      this.paymentRecent.set(payments);
      console.log(payments);
    });

    this.userQuizService.getLatestQuizzes().subscribe((latestQuizzes) => {
      this.latestQuizzes.set(latestQuizzes);
      console.log(latestQuizzes);
    });
  }
}
