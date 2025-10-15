import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { ViewUserDto } from '../../core/models/user-model';
import { TabsModule } from 'primeng/tabs';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subject, takeUntil, tap } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { IftaLabelModule } from 'primeng/iftalabel';
import { UserService } from '../../core/services/user.service';
import { MessageService } from 'primeng/api';
import { SettingsService } from '../../core/services/settings.service';
import { passwordValidator } from '../../core/validators/settings-validator';
import { PasswordModule } from 'primeng/password';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  imports: [
    TabsModule,
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    InputTextModule,
    IftaLabelModule,
    PasswordModule,
  ],
})
export class MyAccountComponent implements OnInit, OnDestroy {
  private authService = inject(AuthService);
  private userService = inject(UserService);
  private fb = inject(FormBuilder);

  private settingsService = inject(SettingsService);

  private destroy$ = new Subject<void>();

  private cd = inject(ChangeDetectorRef);

  private msgService = inject(MessageService);

  user = signal<ViewUserDto | null>(null);

  formInfo!: FormGroup;

  formChangePwd!: FormGroup;

  ngOnInit(): void {
    this.authService
      .me()
      .pipe(takeUntil(this.destroy$))
      .subscribe((res) => {
        this.user.set(res);
        this.cd.markForCheck();

        this.formInfo = this.fb.group({
          name: [this.user()?.name],
          surname: [this.user()?.surname],
          userName: [this.user()?.userName, Validators.required],
          phoneNumber: [this.user()?.phoneNumber],
          email: [this.user()?.email, Validators.required],
        });

        this.settingsService.getUserPolicy().subscribe((res) => {
          if (!res.isUserNameUpdateEnabled) this.formInfo.get('userName')?.disable();
          if (!res.isEmailUpdateEnabled) this.formInfo.get('email')?.disable();
        });

        this.formChangePwd = this.fb.group({
          nowPassword: ['', Validators.required, passwordValidator(this.settingsService)],
          newPassword: ['', Validators.required, passwordValidator(this.settingsService)],
          confirmNewPassword: ['', Validators.required],
        });
      });
  }

  saveInfo() {
    this.userService.updatePersonalInfo(this.user()?.userId!, this.formInfo.value).subscribe({
      next: (viewUser) => {
        this.msgService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: 'Đã lưu thông tin cá nhân',
        });
      },
    });
  }

  changePwd() {
    console.log(this.formChangePwd.value);
    this.userService.changePassword(this.user()?.userId!, this.formChangePwd.value).subscribe({
      next: (result) => {
        this.msgService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: 'Đổi mật khẩu thành công',
        });
      },
      error: (err) => {
        if (err.error && err.error.errors) {
          const validationErrors = err.error.errors;

          Object.keys(validationErrors).forEach((field) => {
            const normalizedKey = field.charAt(0).toLowerCase() + field.slice(1);
            const control = this.formChangePwd.get(normalizedKey);
            if (control) {
              control.setErrors({ serverError: validationErrors[field][0] });
            }
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

  get userName() {
    return this.formInfo.get('userName');
  }

  get email() {
    return this.formInfo.get('email');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
