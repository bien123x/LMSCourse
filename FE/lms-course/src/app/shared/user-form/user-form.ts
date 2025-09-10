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
          phoneNumber: ['', [Validators.required]],
          isActive: [true],
          roles: [[]],
        });
      } else if (this.mode() === 'edit') {
        this.viewUser.set(this.config.data.viewUser);
        const rolesArray: string[] | undefined = this.viewUser()?.roles
          ? this.viewUser()?.roles.split(', ')
          : [];
        this.userForm = this.fb.group({
          userName: [this.viewUser()?.userName, [Validators.required]],
          name: [this.viewUser()?.name],
          surname: [this.viewUser()?.surname],
          email: [this.viewUser()?.email, [Validators.required, Validators.email]],
          phoneNumber: [this.viewUser()?.phoneNumber, [Validators.required]],
          isActive: [this.viewUser()?.isActive],
          roles: [rolesArray],
        });
      }
    }
  }

  get userName() {
    return this.userForm.get('userName');
  }

  close() {
    this.ref.close();
  }
  save() {
    if (this.userForm.valid && this.mode() === 'add') {
      this.userDto.set(this.userForm.value);
      this.ref.close(this.userDto());
    }
    if (this.userForm.valid && this.mode() === 'edit') {
      this.editUserDto.set({ ...this.userForm.value, userId: this.viewUser()?.userId });
      this.ref.close(this.editUserDto());
    } else {
      this.userForm.markAllAsTouched();
    }
    console.log(this.userForm.value);
  }
}
