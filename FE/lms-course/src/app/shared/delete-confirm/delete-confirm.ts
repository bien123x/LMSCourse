import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Component, inject, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RadioButtonModule } from 'primeng/radiobutton';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { RoleService } from '../../core/services/role.service';
import { Role } from '../../core/models/role-model';

@Component({
  selector: 'app-delete-confirm',
  templateUrl: './delete-confirm.html',
  imports: [ButtonModule, RadioButtonModule, FormsModule, SelectModule],
})
export class DeleteConfirmComponent {
  private roleService = inject(RoleService);
  mode = signal<string>('');
  msg = signal<string>('');
  isAssign: string = '';
  rolesMinus = signal<Role[]>([]);
  selectedRole = 0;
  constructor(private ref: DynamicDialogRef, private config: DynamicDialogConfig) {
    if (config.data) {
      this.mode.set(config.data.mode);
      if (this.mode() === 'role-delete') {
        this.msg.set(config.data.msg);
        if (this.msg()) {
          this.roleService.getRolesMinusRole(config.data.role.roleId).subscribe((rolesMinus) => {
            this.rolesMinus.set(rolesMinus);
            console.log(this.rolesMinus());
          });
        }
      }
    }
  }

  close() {
    this.ref.close();
    // console.log(this.isAssign);
    // console.log(this.selectedRole);
  }
  delete() {
    if (this.isAssign != '') {
      this.ref.close(this.selectedRole);
    } else {
      this.ref.close(this.isAssign);
    }
  }
}
