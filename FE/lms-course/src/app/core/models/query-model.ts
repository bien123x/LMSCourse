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
