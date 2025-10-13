import { CourseDto } from './course-model';

export interface PaymentDto {
  userId: number;
  amount: number;
  method: string;
  status: string;
  appTransId: string;
  courseDtos: CourseDto[];
}
