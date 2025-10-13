import { DashboardStudentComponent } from './../../features/student/dashboard-student/dashboard-student.component';
export interface CourseFiltersDto {
  categories: CategoryDto[];
  teachers: TeacherDto[];
  levels: LevelDto[];
  languages: LanguageDto[];
}
export interface CategoryDto {
  categoryId: number;
  name: string;
  count: number;
}
export interface TeacherDto {
  teacherId: number;
  name: string;
  count: number;
}
export interface LevelDto {
  levelId: number;
  name: string;
  count: number;
}
export interface LanguageDto {
  languageId: number;
  name: string;
  count: number;
}

export interface CourseDto {
  courseId: number;
  title: string;
  isPublic: boolean;
  maxStudents: number;
  shortDescription: string;
  description: string;
  createdAt: Date;
  updatedAt: Date;
  teacherId: number;
  teacherName: string;
  categoryId: number;
  categoryName: string;
  levelId: number;
  levelName: string;
  languageId: number;
  languageName: string;
  faqGroups: FaqGroupDto[];
  courseTopics: CourseTopicDto[];
  thumbnailUrl: string;
  videoType: string;
  videoUrl: string;
  isFree: boolean;
  price: number;
  hasDiscount: boolean;
  discountPrice: number;
  isLifetime: boolean;
  durationInMonths: number;
}

export interface CourseTopicDto {
  courseTopicId: number;
  title: string;
  lessons: LessonDto[];
}
export interface LessonDto {
  lessonId: number;
  title: string;
}

export interface FaqGroupDto {
  faqGroupId: number;
  title: string;
  faqItems: FaqItemDto[];
}
export interface FaqItemDto {
  faqItemId: number;
  question: string;
  answer: string;
}

// DTO chi tiết (đã đăng ký)
export interface LessonDetailDto extends LessonDto {
  lessonContent?: string;
  description?: string;
  isFreeOrPremium?: boolean;
  courseTopicId?: number;
}

// Course có bài học chi tiết
export interface CourseEnrolledDto extends CourseDto {
  courseTopics: CourseTopicDetailDto[];
}

export interface CourseTopicDetailDto {
  courseTopicId: number;
  title: string;
  lessons: LessonDetailDto[];
}

export interface EnrollmentDto {
  enrollmentId: number;
  userId: number;
  courseId: number;
  course: CourseEnrolledDto;
  status: string;
  enrollDate: Date;
  progess: number;
}

export interface DashboardEnrollmentCourseDto {
  countEnrolledCourse: number;
  countActiveCourse: number;
  countCompleteCourse: number;
  recentEnrolledCourses: EnrollmentDto[];
}
