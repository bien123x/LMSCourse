import { Component, model } from '@angular/core';
import { LatestQuizDto } from '../../../../core/models/quiz-model';
import { BadgeModule } from 'primeng/badge';

@Component({
  selector: 'app-latest-quiz',
  templateUrl: './latest-quiz.component.html',
  imports: [BadgeModule],
})
export class LatestQuizComponent {
  latestQuiz = model<LatestQuizDto>();
}
