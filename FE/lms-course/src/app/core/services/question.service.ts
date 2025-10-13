import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuestionViewDto } from '../models/quiz-model';

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Questions';

  getQuestionsByQuizId(quizId: number): Observable<QuestionViewDto[]> {
    return this.http.get<QuestionViewDto[]>(`${this.apiUrl}/get-questions-with-answers/${quizId}`);
  }
}
