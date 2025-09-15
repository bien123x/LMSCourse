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
import { Ripple } from 'primeng/ripple';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterDto } from '../../../core/models/auth-model';

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
  ],
  providers: [MessageService],
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private msgService = inject(MessageService);

  registerDto = signal<RegisterDto>({
    UserName: '',
    Email: '',
    PasswordHash: '',
  });

  onSubmit(form: NgForm) {
    console.log(this.registerDto());
    this.authService.register(this.registerDto()).subscribe({
      next: (res) => {
        console.log(res);
        this.router.navigate(['/auth/login']);
      },
      error: (err) => {
        console.log('Lỗi đăng kí', err.error);
        if (err.error.errors != null) {
          const e = err.error.errors.join('\n');
          //Toast
          this.msgService.add({
            severity: 'error',
            summary: 'Lỗi',
            detail: e,
            life: 3000,
          });
        }
      },
    });
  }
}
