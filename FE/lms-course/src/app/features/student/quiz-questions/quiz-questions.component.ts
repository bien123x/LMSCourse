import { CertificateDto } from './../../../core/models/certificate-model';
import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { StepperModule } from 'primeng/stepper';
import { ButtonModule } from 'primeng/button';
import { QuestionService } from '../../../core/services/question.service';
import { QuestionViewDto, QuizViewDto } from '../../../core/models/quiz-model';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from '../../../core/services/quiz.service';
import { MessageService } from 'primeng/api';
import { MessageModule } from 'primeng/message';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { AuthService } from '../../../core/services/auth.service';
import { forkJoin, of, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { UserQuizService } from '../../../core/services/user-quiz.service';
import { CardModule } from 'primeng/card';
import { EnrollmentService } from '../../../core/services/enrollment.service';
import { EnrollmentDto } from '../../../core/models/course-model';
import { CertificateService } from '../../../core/services/certificate.service';

@Component({
  selector: 'app-quiz-questions',
  templateUrl: './quiz-questions.component.html',
  imports: [
    StepperModule,
    ButtonModule,
    MessageModule,
    ReactiveFormsModule,
    RadioButtonModule,
    CommonModule,
    CardModule,
  ],
})
export class QuizQuestionsComponent implements OnInit, OnDestroy {
  private questionService = inject(QuestionService);
  private quizService = inject(QuizService);
  private route = inject(ActivatedRoute);
  private msgService = inject(MessageService);
  private authService = inject(AuthService);
  private userQuizService = inject(UserQuizService);
  private enrollmentService = inject(EnrollmentService);
  private certificateService = inject(CertificateService);
  private router = inject(Router);
  quizId = signal<number>(0);
  quiz = signal<QuizViewDto | null>(null);
  questions = signal<QuestionViewDto[]>([]);

  totalSeconds = signal<number>(0);
  timeLeft = signal<string>('');

  private cd = inject(ChangeDetectorRef);

  private destroy$ = new Subject<void>();

  startTime!: Date;

  quizForm!: FormGroup;

  isFinish = false;
  message = signal<string>('');
  score = signal<number>(0);

  enrollment = signal<EnrollmentDto | null>(null);

  certificateDto: CertificateDto = {
    userId: 0,
    courseId: 0,
    teacherId: 0,
  };

  countdownInterval: any;

  private fb = inject(FormBuilder);

  ngOnInit(): void {
    this.startTime = new Date();

    // tối ưu
    this.route.params
      .pipe(
        switchMap((params) => {
          const id = +params['id'];
          this.quizId.set(id);
          return forkJoin({
            questions: this.questionService.getQuestionsByQuizId(id),
            quiz: this.quizService.getQuizById(id),
            user: this.authService.me(),
          });
        }),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: ({ quiz, questions, user }) => {
          this.certificateDto.userId = user.userId;
          this.quiz.set(quiz);
          this.questions.set(questions);
          this.initForm(user.userId);
          this.totalSeconds.set(this.getTotalSeconds(quiz.duration));
          this.startCountdown();
        },
        error: () => {
          this.msgService.add({
            severity: 'error',
            summary: 'Không tìm thấy dữ liệu',
            detail: 'Bài tập không tồn tại',
          });
        },
      });
  }
  initForm(userId: number) {
    this.quizForm = this.fb.group({
      userId: [userId],
      quizId: [this.quizId()],
      startTime: [this.startTime],
      endTime: [null],
      score: [0],
      userAnswers: this.fb.array(
        this.questions().map((q) =>
          this.fb.group({
            questionId: [q.questionId],
            answerId: [null, Validators.required],
          })
        )
      ),
    });
  }

  get userAnswers(): FormArray {
    return this.quizForm.get('userAnswers') as FormArray;
  }

  startCountdown() {
    if (this.countdownInterval) this.stopCountdown();

    this.countdownInterval = setInterval(() => {
      if (this.totalSeconds() > 0) {
        this.totalSeconds.update((t) => t - 1);
        this.timeLeft.set(this.formatTime(this.totalSeconds()));
      } else {
        this.submitQuiz();
      }
    }, 1000);
  }

  stopCountdown() {
    if (this.countdownInterval) {
      clearInterval(this.countdownInterval);
      this.countdownInterval = null;
    }
  }

  getTotalSeconds(duration: string): number {
    const parts = duration.split(':'); // ["02", "30", "00"]

    const hours = parseInt(parts[0], 10);
    const minutes = parseInt(parts[1], 10);
    const seconds = parseInt(parts[2], 10);

    return hours * 3600 + minutes * 60 + seconds;
  }

  formatTime(totalSeconds: number): string {
    const h = Math.floor(totalSeconds / 3600);
    const m = Math.floor((totalSeconds % 3600) / 60);
    const s = totalSeconds % 60;
    return `${this.pad(h)}:${this.pad(m)}:${this.pad(s)}`;
  }

  pad(num: number) {
    return num < 10 ? '0' + num : num.toString();
  }

  submitQuiz() {
    this.stopCountdown();
    this.quizForm.get('endTime')?.setValue(new Date());

    this.userQuizService
      .createUserQuizWithUserAnswers(this.quizForm.value)
      .pipe(
        switchMap((userQuizViewDto) => {
          this.score.set(userQuizViewDto.score);

          return this.enrollmentService.updateStatusEnrolledCourse(this.quiz()?.courseId!).pipe(
            tap((enrollment) => {
              this.enrollment.set(enrollment);
              this.certificateDto.courseId = enrollment.course.courseId;
              this.certificateDto.teacherId = enrollment.course.teacherId;
            }),
            switchMap((enrollment) => {
              if (enrollment.status === 'Completed') {
                return this.enrollmentService.isExistCertificate(enrollment.enrollmentId).pipe(
                  tap((isExist) => {
                    if (!isExist) {
                      this.certificateService.generateCertificate(this.certificateDto).subscribe();
                      this.msgService.add({
                        severity: 'success',
                        summary: 'Thành công',
                        detail:
                          'Đã hoàn thành khoá học. Vui lòng kiểm tra chứng chỉ để biết thêm chi tiết!',
                        life: 5000,
                      });
                    }
                  })
                );
              }
              return of(null);
            }),
            switchMap(() => this.userQuizService.getIsPassQuiz(userQuizViewDto.userQuizId))
          );
        }),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: (res) => {
          this.message.set(res.message);
          this.isFinish = true;
          this.cd.detectChanges();
          if (res.passed) {
            this.msgService.add({
              severity: 'success',
              summary: 'Qua',
              detail: 'Đã qua bài tập!',
            });
          } else {
            this.msgService.add({
              severity: 'error',
              summary: 'Trượt',
              detail: 'Không qua bài tập!',
            });
          }
        },
      });
  }

  goToDashboard() {
    console.log('ve');
    this.router.navigate(['/student/dashboard']);
  }

  ngOnDestroy(): void {
    this.stopCountdown();
    this.destroy$.next();
    this.destroy$.complete();
  }
}
