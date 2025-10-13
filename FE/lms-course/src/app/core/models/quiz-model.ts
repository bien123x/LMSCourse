export interface QuizViewDto {
  quizId: number;
  courseId: number;
  title: string;
  noOfQuestions: number;
  totalMarks: number;
  passMark: number;
  duration: string;
}
export interface QuestionViewDto {
  questionId: number;
  questionStr: string;
  questionType: string;
  quizId: number;
  answers: AnswerViewDto[];
}

export interface AnswerViewDto {
  answerId: number;
  questionId: number;
  answerStr: string;
  isCorrect: boolean;
}

export interface UserQuizDto {
  userId: number;
  quizId: number;
  score: number;
  startTime: Date;
  endTime: Date;
  userAnswers: UserAnswerDto[];
}

export interface UserAnswerDto {
  questionId: number;
  answerId: number;
}

export interface UserQuizViewDto {
  userQuizId: number;
  userId: number;
  quizId: number;
  score: number;
  startTime: Date;
  endTime: Date;
  userAnswers: UserAnswerDto[];
}

export interface UserAnswerViewDto {
  userAnswerId: number;
  userQuizId: number;
  questionId: number;
  answerId: number;
}

export interface AnswerDto {
  answerId: number;
  questionId: number;
  content: string;
  isCorrect: boolean;
}

export interface QuestionDto {
  questionId: number;
  questionType: string;
  quizId: number;
}

export interface LatestQuizDto {
  userQuizId: number;
  countCorrectAnswer: number;
  titleQuiz: string;
  percentageScore: number;
  noOfQuestions: number
}