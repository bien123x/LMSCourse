import { PERMISSION } from './../../../core/models/constant';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MenuItem, MessageService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { TabsModule } from 'primeng/tabs';
import { InputNumberModule } from 'primeng/inputnumber';
import { IftaLabel } from 'primeng/iftalabel';
import { Checkbox } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { IdentitySettingDto } from '../../../core/models/settings-model';
import { SettingsService } from '../../../core/services/settings.service';
import { Card, CardModule } from 'primeng/card';
import { HasPermissionDirective } from "../../../core/directives/has-permission-directive";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.html',
  imports: [
    MenuModule,
    TabsModule,
    ReactiveFormsModule,
    InputNumberModule,
    ButtonModule,
    IftaLabel,
    Checkbox,
    CardModule,
    HasPermissionDirective
],
})
export class SettingsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private settingsService = inject(SettingsService);
  private msgService = inject(MessageService);
  private cd = inject(ChangeDetectorRef);
  items: MenuItem[] | undefined = undefined;
  menuSelect: string = '';
  form!: FormGroup;
  identitySettingDto: IdentitySettingDto | undefined = undefined;

  PERMISSION = PERMISSION;

  ngOnInit(): void {
    this.settingsService.getIdentitySetting().subscribe({
      next: (res: any) => {
        this.identitySettingDto = res;
        this.form = this.fb.group({
          password: this.fb.group({
            requiredLength: [this.identitySettingDto?.password.requiredLength, Validators.required],
            requiredUniqueChars: [this.identitySettingDto?.password.requiredUniqueChars],
            requireDigit: [this.identitySettingDto?.password.requireDigit],
            requireLowercase: [this.identitySettingDto?.password.requireLowercase],
            requireUppercase: [this.identitySettingDto?.password.requireUppercase],
            requireNonAlphanumeric: [this.identitySettingDto?.password.requireNonAlphanumeric],
            forceUsersToPeriodicallyChangePassword: [
              this.identitySettingDto?.password.forceUsersToPeriodicallyChangePassword,
              Validators.required,
            ],
            passwordChangePeriodDays: [
              this.identitySettingDto?.password.passwordChangePeriodDays,
              [Validators.min(0), Validators.required],
            ],
          }),
          lockout: this.fb.group({
            allowedForNewUsers: [this.identitySettingDto?.lockout.allowedForNewUsers],
            lockoutDuration: [this.identitySettingDto?.lockout.lockoutDuration],
            maxFailedAccessAttempts: [this.identitySettingDto?.lockout.maxFailedAccessAttempts],
          }),
          signIn: this.fb.group({
            requireConfirmedEmail: [this.identitySettingDto?.signIn.requireConfirmedEmail],
            requireEmailVerificationToRegister: [
              this.identitySettingDto?.signIn.requireEmailVerificationToRegister,
            ],
            enablePhoneNumberConfirmation: [
              this.identitySettingDto?.signIn.enablePhoneNumberConfirmation,
            ],
            requireConfirmedPhoneNumber: [
              this.identitySettingDto?.signIn.requireConfirmedPhoneNumber,
            ],
          }),
          user: this.fb.group({
            isUserNameUpdateEnabled: [this.identitySettingDto?.user.isUserNameUpdateEnabled],
            isEmailUpdateEnabled: [this.identitySettingDto?.user.isEmailUpdateEnabled],
          }),
        });

        this.cd.markForCheck();
      },
      error: (err) => {
        console.log(err);
      },
    });

    this.menuSelect = 'identity';
    this.items = [
      { label: 'Tài khoản', command: () => (this.menuSelect = 'account') },
      { label: 'Quản lý định danh', command: () => this.selectIdentity() },
    ];
  }

  selectIdentity() {
    this.menuSelect = 'identity';
  }

  get passwordChangePeriodDays() {
    return this.form.get('password.passwordChangePeriodDays');
  }

  onSave() {
    if (this.form.valid) {
      this.identitySettingDto = { ...this.form.value };
      this.settingsService.updateIdentitySetting(this.identitySettingDto).subscribe({
        next: (res) => {
          this.msgService.add({
            severity: 'success',
            summary: 'Thành công',
            detail: 'Cập nhật thành công',
          });
        },
        error: (err) => {
          console.log(err);
        },
      });
    }
  }
}
