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
