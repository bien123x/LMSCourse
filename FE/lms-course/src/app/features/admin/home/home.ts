import { Component, inject } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  imports: [ButtonModule],
})
export class HomeComponent {
  private confirmationService = inject(ConfirmationService);
  private messageService = inject(MessageService);
  confirmDialog(event: Event) {
    this.confirmationService.confirm({
      target: event.target as HTMLElement, // định vị theo button
      header: 'Xác nhận',
      message: 'gaga',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Save',
        severity: 'danger',
      },
      accept: () => {
        this.messageService.add({
          severity: 'info',
          summary: 'Confirmed',
          detail: 'You have accepted',
        });
      },
      reject: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Rejected',
          detail: 'You have rejected',
          life: 3000,
        });
      },
    });
    this.messageService.add({
      severity: 'success',
      summary: 'Ok',
      icon: 'pi pi-send',
      detail: 'Hhaa',
    });
  }
}
