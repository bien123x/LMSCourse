import { Component, inject, input, OnInit, signal } from '@angular/core';
import { QuizItemComponent } from './quiz-item/quiz-item.component';
import { QuizService } from '../../../../core/services/quiz.service';
import { QuizViewDto } from '../../../../core/models/quiz-model';

@Component({
  selector: 'app-quiz-course',
  templateUrl: './quiz-course.component.html',
  imports: [QuizItemComponent],
})
export class QuizCourseComponent implements OnInit {
  // quizzes = input<QuizViewDto[] | undefined>(undefined);
  private quizService = inject(QuizService);
  courseId = input<number>(1);
  quizzes = signal<QuizViewDto[]>([]);
  ngOnInit(): void {
    this.quizService.getQuizzesByCourseId(this.courseId()).subscribe((res) => {
      this.quizzes.set(res);
      console.log(this.quizzes());
    });
  }
}
