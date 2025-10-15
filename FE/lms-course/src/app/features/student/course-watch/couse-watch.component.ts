import { PERMISSION } from './../../../core/models/constant';
import { QuizViewDto } from './../../../core/models/quiz-model';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from '../../../core/services/course.service';
import {
  CourseDto,
  CourseEnrolledDto,
  EnrollmentDto,
  LessonDetailDto,
} from '../../../core/models/course-model';
import { AccordionModule } from 'primeng/accordion';
import { EnrollmentService } from '../../../core/services/enrollment.service';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { VideoComponent } from './video/video.component';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FormsModule } from '@angular/forms';
import { OverviewCourseComponent } from './overview-course/overview-course.component';
import { FAQCourseComponent } from './faq-course/faq-course.component';
import { QuizCourseComponent } from './quiz-course/quiz-course.component';
import { QuizService } from '../../../core/services/quiz.service';
import { ProgressBarModule } from 'primeng/progressbar';
import { HasPermissionDirective } from "../../../core/directives/has-permission-directive";

@Component({
  selector: 'app-course-watch',
  templateUrl: './course-watch.component.html',
  styleUrl: './course-watch.component.css',
  imports: [
    AccordionModule,
    ButtonModule,
    SelectButtonModule,
    FormsModule,
    OverviewCourseComponent,
    FAQCourseComponent,
    QuizCourseComponent,
    ProgressBarModule,
    HasPermissionDirective
],
})
export class CourseWatchComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  private enrollmentService = inject(EnrollmentService);
  private dialogService = inject(DialogService);
  private quizService = inject(QuizService);
  ref = signal<DynamicDialogRef | undefined>(undefined);
  enrollment = signal<EnrollmentDto | undefined>(undefined);
  course = signal<CourseEnrolledDto | undefined>(undefined);
  courseId: number = 0;
  selectOptions: any[] = [
    { label: 'Tổng quan', value: 'overview' },
    { label: 'Câu hỏi thường gặp', value: 'faq' },
    { label: 'Bài tập', value: 'quiz' },
  ];
  selectOption: string = 'overview';

  quizzes: QuizViewDto[] = [];

  PERMISSION = PERMISSION;

  ngOnInit(): void {
    this.route.params.subscribe((param) => {
      this.courseId = +param['id'];

      this.enrollmentService.getEnrollmentById(this.courseId).subscribe((res) => {
        this.enrollment.set(res);
        this.course.set(res.course);
      });

      this.quizService.getQuizzesByCourseId(this.courseId).subscribe((res) => {
        this.quizzes = res;
      });
    });
  }

  watchVideo(lesson: LessonDetailDto) {
    console.log(lesson);
    this.ref.set(
      this.dialogService.open(VideoComponent, {
        header: lesson.title,
        width: '60%',
        modal: true,
        data: {
          videoUrl: lesson.lessonContent,
        },
      })
    );
    this.ref()?.onClose.subscribe();
  }
}
