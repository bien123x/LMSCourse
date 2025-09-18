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
import { Toast } from 'primeng/toast';
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
    Toast,
    RouterLink,
    DialogModule,
    ReactiveFormsModule,
  ],
  providers: [MessageService],
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
      nowPassword: [
        '',
        {
          asyncValidators: passwordValidator(this.settingService),
          updateOn: 'blur',
        },
      ],
      newPassword: [
        '',
        {
          asyncValidators: passwordValidator(this.settingService),
          updateOn: 'blur',
        },
      ],
      confirmNewPassword: [
        '',
        {
          asyncValidators: passwordValidator(this.settingService),
          updateOn: 'blur',
        },
      ],
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
          this.router.navigate(['/home']);
        }
      },
      error: (err) => {
        console.log(err);
        //Toast
        if (!err.error.success) {
          this.msgService.add({
            severity: 'error',
            summary: 'Lỗi',
            detail: err.error.message,
            life: 3000,
          });
        } else if (err.error.success) {
          this.msgService.add({
            severity: 'info',
            summary: 'Thông tin',
            detail: err.error.message,
            life: 3000,
          });
        }
      },
    });
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
          this.msgService.add({
            severity: 'error',
            summary: 'Thất bại',
            detail: err.error.message,
          });
        },
      });
    }
  }
}
