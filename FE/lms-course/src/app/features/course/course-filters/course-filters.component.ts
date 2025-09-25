import { Component, input, OnInit, output } from '@angular/core';
import { CourseFiltersDto } from '../../../core/models/course-model';
import { AccordionModule } from 'primeng/accordion';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule } from '@angular/forms';
import { CourseFilterRequest } from '../../../core/models/query-model';

@Component({
  selector: 'app-course-filters',
  templateUrl: './course-filters.component.html',
  imports: [AccordionModule, CheckboxModule, FormsModule],
})
export class CourseFiltersComponent implements OnInit {
  courseFilters = input<CourseFiltersDto | undefined>(undefined);
  selectedFilters = output<any>();

  selected: CourseFilterRequest = {
    teacherIds: [],
    categoryIds: [],
    levelIds: [],
    languageIds: [],
  };

  ngOnInit(): void {
    console.log(this.courseFilters());
  }

  toggleFilter(event: any) {
    console.log('Event Filter:', event);
    this.selectedFilters.emit(this.selected);
  }
}
