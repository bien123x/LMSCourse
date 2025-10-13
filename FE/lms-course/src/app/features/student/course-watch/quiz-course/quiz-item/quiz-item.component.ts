import { Component, inject, input } from '@angular/core';
import { QuizViewDto } from '../../../../../core/models/quiz-model';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quiz-item',
  templateUrl: './quiz-item.component.html',
  imports: [ButtonModule],
})
export class QuizItemComponent {
  quizItem = input<QuizViewDto | undefined>(undefined);
  private router = inject(Router);

  quizQestion(quizId: number) {
    this.router.navigate(['/student/quiz-question', quizId]);
  }
}
