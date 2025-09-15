import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MenuItem, MessageService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { TabsModule } from 'primeng/tabs';
import { PasswordPolicy } from '../../core/models/settings-model';
import { SettingsService } from '../../core/services/settings.service';
import { InputNumberModule } from 'primeng/inputnumber';
import { IftaLabel } from 'primeng/iftalabel';
import { Checkbox } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { Toast } from 'primeng/toast';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.html',
  imports: [
    MenuModule,
    TabsModule,
    ReactiveFormsModule,
    InputNumberModule,
    IftaLabel,
    Checkbox,
    ButtonModule,
    Toast,
  ],
  providers: [MessageService],
})
export class SettingsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private settingsService = inject(SettingsService);
  private msgService = inject(MessageService);
  items: MenuItem[] | undefined = undefined;
  menuSelect: string = '';
  form!: FormGroup;
  passwordPolicy: PasswordPolicy | undefined = undefined;
  ngOnInit(): void {
    this.settingsService.getPasswordPolicy().subscribe({
      next: (res) => {
        this.passwordPolicy = res;
        console.log(this.passwordPolicy);
        this.form = this.fb.group({
          minLength: [this.passwordPolicy.minLength],
          requiredUniqueChars: [this.passwordPolicy.requiredUniqueChars],
          requireDigit: [this.passwordPolicy.requireDigit],
          requireLowercase: [this.passwordPolicy.requireLowercase],
          requireUppercase: [this.passwordPolicy.requireUppercase],
          requireNonAlphanumeric: [this.passwordPolicy.requireNonAlphanumeric],
        });
      },
      error: (err) => {
        console.log(err);
      },
    });

    this.menuSelect = 'account';
    this.items = [
      { label: 'Tài khoản', command: () => (this.menuSelect = 'account') },
      { label: 'Quản lý định danh', command: () => (this.menuSelect = 'identity') },
    ];
  }

  onSave() {
    console.log('save', this.form.value);
    this.passwordPolicy = { ...this.form.value };
    this.settingsService.updatePasswordPolicy(this.passwordPolicy!).subscribe({
      next: (res) => {
        this.msgService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: res.message,
        });
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
