import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LatestQuizDto, UserQuizDto, UserQuizViewDto } from '../models/quiz-model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserQuizService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/UserQuizzes';

  createUserQuizWithUserAnswers(userQuizDto: UserQuizDto): Observable<UserQuizViewDto> {
    return this.http.post<UserQuizViewDto>(
      `${this.apiUrl}/create-user-quiz-with-user-answer`,
      userQuizDto
    );
  }

  getIsPassQuiz(userQuizId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/is-pass-quiz/${userQuizId}`);
  }

  getLatestQuizzes(): Observable<LatestQuizDto[]> {
    return this.http.get<LatestQuizDto[]>(`${this.apiUrl}/get-lastest-quizzes`);
  }
}
