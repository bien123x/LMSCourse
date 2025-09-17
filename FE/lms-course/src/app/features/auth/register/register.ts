import { join } from 'node:path';
import { Component, inject, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Router, RouterLink } from '@angular/router';
import { IftaLabelModule } from 'primeng/iftalabel';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule, NgForm } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { MessageService } from 'primeng/api';
import { Toast } from 'primeng/toast';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterDto } from '../../../core/models/auth-model';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-register',
  templateUrl: './register.html',
  imports: [
    ButtonModule,
    IftaLabelModule,
    InputTextModule,
    FormsModule,
    PasswordModule,
    Toast,
    RouterLink,
    CardModule,
  ],
  providers: [MessageService],
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private msgService = inject(MessageService);

  errorMsg = signal<string>('');

  registerDto = signal<RegisterDto>({
    UserName: '',
    Email: '',
    PasswordHash: '',
  });

  onSubmit(form: NgForm) {
    this.authService.register(this.registerDto()).subscribe({
      next: (res) => {
        this.msgService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: 'Đăng ký tài khoản thành công. Vui lòng kiểm tra email để xác minh.',
          life: 3000,
        });

        this.router.navigate(['/auth/login']);
      },
      error: (err) => {
        if (err.error) {
          if (err.error.errors && Array.isArray(err.error.errors)) {
            this.errorMsg.set(err.error.errors.join('\n'));
          } else if (typeof err.error === 'string') {
            this.errorMsg.set(err.error);
          } else if (err.error.message) {
            this.errorMsg.set(err.error.message);
          }
        }
      },
    });
  }
}
