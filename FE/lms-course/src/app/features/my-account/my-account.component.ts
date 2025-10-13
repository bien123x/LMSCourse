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
