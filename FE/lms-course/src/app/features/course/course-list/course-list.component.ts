import { Component, inject, input, model, OnInit, output } from '@angular/core';
import { CourseDto } from '../../../core/models/course-model';
import { DataViewModule } from 'primeng/dataview';
import { CommonModule } from '@angular/common';
import { Tag } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { SelectItem } from 'primeng/api';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrl: './course-list.component.css',
  imports: [CommonModule, DataViewModule, Tag, ButtonModule, SelectModule, FormsModule],
})
export class CourseListComponent implements OnInit {
  courses = model<CourseDto[]>([]);
  courseId = output<number>();
  rows = model<number>(5);
  totalRecodes = model<number>(0);
  pageNumber = output<number>();

  sortOptions!: SelectItem[];

  sortOrder: number = 1;

  sortField: string = 'title';
  sortKey: string = '';

  sortChange = output<{ field: string; order: number }>();

  ngOnInit(): void {
    this.sortOptions = [
      { label: 'Giá cao đến thấp', value: '!price' },
      { label: 'Giá thấp đến cao', value: 'price' },
    ];
  }

  loadLazy(event: any) {
    console.log(event);
    this.pageNumber.emit(event?.first != null && event?.rows ? event.first / event.rows + 1 : 1);

    this.sortChange.emit({ field: this.sortField, order: this.sortOrder });
  }

  onSortChange(event: any) {
    let value = event.value;

    if (value.indexOf('!') === 0) {
      this.sortOrder = -1;
      this.sortField = value.substring(1, value.length);
    } else {
      this.sortOrder = 1;
      this.sortField = value;
    }
  }

  viewDetail(item: any) {
    this.courseId.emit(item);
  }
}
