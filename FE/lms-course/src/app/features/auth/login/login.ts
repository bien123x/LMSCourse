import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { ButtonModule } from 'primeng/button';
import { LoginDto } from '../../../core/models/auth-model';
import { Router, RouterLink } from '@angular/router';
import { IftaLabelModule } from 'primeng/iftalabel';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule, NgForm } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { MessageService } from 'primeng/api';
import { Toast } from 'primeng/toast';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  imports: [
    ButtonModule,
    IftaLabelModule,
    InputTextModule,
    FormsModule,
    PasswordModule,
    Toast,
    RouterLink,
  ],
  providers: [MessageService],
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private msgService = inject(MessageService);
  loginDto = signal<LoginDto>({
    UserNameOrEmail: 'admin',
    Password: '123',
  });

  onSubmit(form: NgForm) {
    this.authService.login(this.loginDto()).subscribe({
      next: (res) => {
        this.router.navigate(['/home']);
      },
      error: (err) => {
        //Toast
        this.msgService.add({
          severity: 'error',
          summary: 'Lỗi',
          detail: 'Sai thông tin đăng nhập!',
          life: 3000,
        });
      },
    });
  }
}
