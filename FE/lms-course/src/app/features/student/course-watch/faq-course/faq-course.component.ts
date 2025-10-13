import { Component, input } from "@angular/core";
import { FaqGroupDto } from "../../../../core/models/course-model";
import { AccordionModule } from "primeng/accordion";

@Component({
    selector: 'app-faq-course',
    templateUrl: './faq-course.component.html',
    imports: [AccordionModule]
})
export class FAQCourseComponent {
    faqGroups = input<FaqGroupDto[] | undefined>(undefined);
}