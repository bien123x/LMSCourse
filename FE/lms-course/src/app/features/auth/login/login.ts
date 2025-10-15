import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { ButtonModule } from 'primeng/button';
import { LoginDto } from '../../../core/models/auth-model';
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
import { UserService } from '../../../core/services/user.service';
import { ChangePasswordDto } from '../../../core/models/user-model';
import { DialogModule } from 'primeng/dialog';
import { passwordValidator } from '../../../core/validators/settings-validator';
import { SettingsService } from '../../../core/services/settings.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  imports: [
    ButtonModule,
    IftaLabelModule,
    InputTextModule,
    FormsModule,
    PasswordModule,
    RouterLink,
    DialogModule,
    ReactiveFormsModule,
  ],
})
export class LoginComponent implements OnInit {
  private authService = inject(AuthService);
  private userService = inject(UserService);
  private settingService = inject(SettingsService);
  private router = inject(Router);
  private msgService = inject(MessageService);
  private fb = inject(FormBuilder);
  loginDto = signal<LoginDto>({
    UserNameOrEmail: 'admin',
    Password: '123',
  });

  changePwdDto = signal<ChangePasswordDto>({
    nowPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  });

  visibleDialogChangePwd = signal<boolean>(false);
  userId: number = 0;

  formChangePwd!: FormGroup;

  ngOnInit(): void {
    this.formChangePwd = this.fb.group({
      nowPassword: ['', [Validators.required], passwordValidator(this.settingService)],
      newPassword: ['', [Validators.required], passwordValidator(this.settingService)],
      confirmNewPassword: ['', Validators.required],
    });
  }

  get visibleDialogPwd() {
    return this.visibleDialogChangePwd();
  }
  set visibleDialogPwd(val: boolean) {
    this.visibleDialogChangePwd.set(val);
  }

  onSubmit(form: NgForm) {
    this.authService.login(this.loginDto()).subscribe({
      next: (res) => {
        console.log(res);
        if (res.requirePasswordChange == true) {
          this.userId = res.userId;
          this.visibleDialogChangePwd.set(true);
        } else {
          console.log('vao');
          if (this.authService.hasRole('Admin')) {
            this.router.navigate(['/admin']);
          } else if (this.authService.hasRole('Teacher')) {
            this.router.navigate(['/teacher']);
          } else if (this.authService.hasRole('Student')) {
            this.router.navigate(['/student']);
          }
        }
      },
      error: (err) => {
        const errorBody = err?.error; // dùng optional chaining

        if (errorBody && typeof errorBody === 'object' && 'success' in errorBody) {
          if (errorBody.success == false) {
            this.msgService.add({
              severity: 'error',
              summary: 'Lỗi',
              detail: errorBody.message,
            });
          } else if (errorBody.success == true) {
            this.msgService.add({
              severity: 'info',
              summary: 'Thông tin',
              detail: errorBody.message,
            });
          }
        } else {
          this.msgService.add({
            severity: 'info',
            summary: 'Thông tin',
            detail: err.error,
          });
        }
      },
    });
  }

  get nowPassword() {
    return this.formChangePwd.get('nowPassword');
  }

  get newPassword() {
    return this.formChangePwd.get('newPassword');
  }

  get confirmNewPassword() {
    return this.formChangePwd.get('confirmNewPassword');
  }

  changePwdClick() {
    if (this.formChangePwd.valid) {
      console.log(this.formChangePwd.value);
      this.changePwdDto.set(this.formChangePwd.value);
      this.userService.changePassword(this.userId, this.changePwdDto()).subscribe({
        next: (result) => {
          this.msgService.add({
            severity: 'success',
            summary: 'Thành công',
            detail: result.message,
          });
          this.visibleDialogChangePwd.set(false);
        },
        error: (err) => {
          console.log(err);
          if (err.status && err.status == 400 && err.error?.errors) {
            const errors = err.error?.errors;
            Object.keys(errors).forEach((key) => {
              const keyFirstLower = key.charAt(0).toLowerCase() + key.slice(1);
              const control = this.formChangePwd.get(keyFirstLower);
              if (control) {
                control.setErrors({ serverError: errors[key][0] });
              }
            });
          } else if (err.error) {
            this.msgService.add({
              severity: 'error',
              summary: 'Thất bại',
              detail: err.error.message,
            });
          }
        },
      });
    }
  }
}
