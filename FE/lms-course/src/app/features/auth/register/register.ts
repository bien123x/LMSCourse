import { join } from 'node:path';
import { Component, inject, signal, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Router, RouterLink } from '@angular/router';
import { IftaLabelModule } from 'primeng/iftalabel';
import { InputTextModule } from 'primeng/inputtext';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  NgForm,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../../core/services/auth.service';
import { CardModule } from 'primeng/card';
import { SettingsService } from '../../../core/services/settings.service';
import { passwordValidator } from '../../../core/validators/settings-validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.html',
  imports: [
    ButtonModule,
    IftaLabelModule,
    InputTextModule,
    FormsModule,
    PasswordModule,
    RouterLink,
    CardModule,
    ReactiveFormsModule,
  ],
  providers: [MessageService],
})
export class RegisterComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  private msgService = inject(MessageService);
  private fb = inject(FormBuilder);
  private settingsService = inject(SettingsService);
  formRegister!: FormGroup;

  errorMsg = signal<string>('');

  ngOnInit(): void {
    this.formRegister = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      passwordHash: ['', [Validators.required], [passwordValidator(this.settingsService)]],
    });
  }

  get userName() {
    return this.formRegister.get('userName');
  }

  get email() {
    return this.formRegister.get('email');
  }

  get password() {
    return this.formRegister.get('passwordHash');
  }

  get passwordPolicy() {
    const errors = this.password?.getError('passwordPolicy');
    return Array.isArray(errors) ? errors.join('\n') : null;
  }

  onSubmit() {
    this.authService.register(this.formRegister.value).subscribe({
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
        console.log(err);
        console.log(err.error?.errors);
        if (err.status && err.status == 400 && err.error?.errors) {
          const errors = err.error.errors;
          Object.keys(errors).forEach((key) => {
            const normalizedKey = key.charAt(0).toLowerCase() + key.slice(1);
            const control = this.formRegister.get(normalizedKey);
            // const control = this.formRegister.get(key);
            if (control) {
              control.setErrors({ server: errors[key][0] });
              console.log(control);
            }
          });
        }
      },
    });
  }
}
