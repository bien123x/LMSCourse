import { Component, model } from "@angular/core";
import { PaymentDto } from "../../../../core/models/payment-model";
import { MessageModule } from "primeng/message";
import { BadgeModule } from "primeng/badge";
import { CurrencyPipe } from "@angular/common";

@Component({
    selector: 'app-invoice',
    templateUrl: './invoice.component.html',
    imports: [MessageModule, BadgeModule, CurrencyPipe]
})
export class InvoiceComponent {
    invoice = model<PaymentDto>();
}