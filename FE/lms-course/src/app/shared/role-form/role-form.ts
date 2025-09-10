import { Component, inject, signal } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.html',
  imports: [ButtonModule, FormsModule, ReactiveFormsModule, InputTextModule],
})
export class RoleFormComponent {
  roleName = new FormControl('');
  mode = signal<string>('add');
  constructor(private ref: DynamicDialogRef, private config: DynamicDialogConfig) {
    if (config.data) {
      this.mode.set(config.data.mode);
      if (this.mode() === 'edit') {
        const role = config.data.role;
        this.roleName.setValue(role.roleName);
      }
    }
  }
  close() {
    this.ref.close();
  }
  save() {
    this.ref.close(this.roleName.value);
  }
}
