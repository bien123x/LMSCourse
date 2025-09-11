import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { TableModule } from 'primeng/table';
import { TabsModule } from 'primeng/tabs';
import { UserService } from '../../core/services/user.service';
import { EditUserDto, UserDto, ViewUserDto } from '../../core/models/user-model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IftaLabel } from 'primeng/iftalabel';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.html',
  imports: [
    ButtonModule,
    TabsModule,
    CommonModule,
    ReactiveFormsModule,
    IftaLabel,
    InputTextModule,
    PasswordModule,
    CheckboxModule,
    TableModule,
  ],
})
export class UserFormComponent implements OnInit {
  private ref = inject(DynamicDialogRef);
  private config = inject(DynamicDialogConfig);
  private userService = inject(UserService);
  private fb = inject(FormBuilder);

  mode = signal<string>('');
  value = signal<number>(0);
  userForm!: FormGroup;
  rolesName = signal<string[]>([]);

  userDto = signal<UserDto | undefined>(undefined);
  editUserDto = signal<EditUserDto | undefined>(undefined);
  viewUser = signal<ViewUserDto | undefined>(undefined);

  general = signal<any>([]);

  ngOnInit(): void {
    if (this.config.data) {
      this.mode.set(this.config.data.mode);
      this.rolesName.set(this.config.data.rolesName);
      if (this.mode() === 'add') {
        this.userForm = this.fb.group({
          userName: ['', [Validators.required]],
          name: [''],
          passwordHash: ['', [Validators.required]],
          surname: [''],
          email: ['', [Validators.required, Validators.email]],
          phoneNumber: [
            '',
            [Validators.required, Validators.minLength(10), Validators.maxLength(10)],
          ],
          isActive: [true],
          roles: [[]],
        });
      } else if (this.mode() === 'edit' || this.mode() === 'viewDetail') {
        console.log(this.config.data);
        this.viewUser.set(this.config.data.viewUser);
        const rolesArray: string[] | undefined = this.viewUser()?.roles
          ? this.viewUser()?.roles.split(', ')
          : [];
        this.userForm = this.fb.group({
          userName: [this.viewUser()?.userName, [Validators.required]],
          name: [this.viewUser()?.name],
          surname: [this.viewUser()?.surname],
          email: [this.viewUser()?.email, [Validators.email]],
          phoneNumber: [
            this.viewUser()?.phoneNumber,
            [Validators.required, Validators.minLength(10), Validators.maxLength(10)],
          ],
          isActive: [this.viewUser()?.isActive],
          roles: [rolesArray],
        });

        if (this.mode() === 'viewDetail') {
          this.userForm.disable();
          this.general.set([
            { label: 'Tạo bởi', value: this.viewUser()?.createBy },
            { label: 'Thời gian tạo', value: this.viewUser()?.creationTime },
            { label: 'Cập nhật gần nhất', value: this.viewUser()?.modificationTime },
            { label: 'Cập nhật bởi', value: this.viewUser()?.modifiedBy },
            { label: 'Thời điểm mở khoá', value: this.viewUser()?.lockoutEndTime },
            { label: 'Số lần đăng nhập thất bại', value: this.viewUser()?.failedAccessCount },
          ]);
        }
      }
    }
  }

  get userName() {
    return this.userForm.get('userName');
  }
  get passwordHash() {
    return this.userForm.get('passwordHash');
  }
  get email() {
    return this.userForm.get('email');
  }
  get phoneNumber() {
    return this.userForm.get('phoneNumber');
  }
  close() {
    this.ref.close();
  }
  save() {
    if (this.userForm.valid) {
      if (this.mode() === 'add') {
        this.userDto.set(this.userForm.value);
        this.ref.close(this.userDto());
      } else if (this.mode() === 'edit') {
        this.editUserDto.set({ ...this.userForm.value, userId: this.viewUser()?.userId });
        this.ref.close(this.editUserDto());
      }
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}
