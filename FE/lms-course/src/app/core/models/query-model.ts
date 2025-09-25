export interface QueryDto {
  pageNumber: number;
  pageSize: number;
  sorts: SortField[];
  filters: FilterField[];
}

export interface SortField {
  field: string;
  order: string;
}

export interface FilterField {
  field: string;
  value: string;
}

export interface CourseFilterRequest {
  teacherIds: number[];
  categoryIds: number[];
  levelIds: number[];
  languageIds: number[];
}

export interface QueryCourseDto {
  pageNumber: number;
  pageSize: number;
  sortField: string;
  sortOrder: number;
  courseFilterRequest: CourseFilterRequest;
}
