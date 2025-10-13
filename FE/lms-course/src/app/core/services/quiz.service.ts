import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuizViewDto } from '../models/quiz-model';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Quiz';

  getQuizzesByCourseId(courseId: number): Observable<QuizViewDto[]> {
    return this.http.get<QuizViewDto[]>(`${this.apiUrl}/get-quizzes-by-courseId/${courseId}`);
  }
  getQuizById(quizId: number): Observable<QuizViewDto> {
    return this.http.get<QuizViewDto>(`${this.apiUrl}/get-quiz-by-id/${quizId}`);
  }
}
