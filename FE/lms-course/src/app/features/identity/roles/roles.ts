import { TableModule } from 'primeng/table';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { RoleService } from '../../../core/services/role.service';
import { Menu } from 'primeng/menu';
import { MenuItem, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { RoleFormComponent } from '../../../shared/role-form/role-form';
import { RoleDto, ViewRolesDto } from '../../../core/models/role-model';
import { ToastModule } from 'primeng/toast';
import { PermissionFormComponent } from '../../../shared/permission-form/permission-form';
import { DeleteConfirmComponent } from '../../../shared/delete-confirm/delete-confirm';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.html',
  imports: [ButtonModule, TableModule, CommonModule, Menu, ToastModule],
  providers: [DialogService, MessageService],
})
export class RolesComponent implements OnInit {
  private roleService = inject(RoleService);
  private dialogService = inject(DialogService);
  private msgService = inject(MessageService);

  roles = signal<ViewRolesDto[]>([]);

  permissions = signal<string[]>([]);

  totalRoles = computed<number>(() => {
    return this.roles().length;
  });

  menuItems = signal<MenuItem[]>([]);

  currentRole = signal<any | null>(null);

  ref = signal<DynamicDialogRef | undefined>(undefined);

  ngOnInit(): void {
    this.roleService.getRoles().subscribe((res) => {
      this.roles.set(res);
      console.log(res);
    });
  }

  deleteRole(viewRoleDto: ViewRolesDto) {
    this.roleService.getCountUser(viewRoleDto.roleId).subscribe((res) => {
      this.ref.set(
        this.dialogService.open(DeleteConfirmComponent, {
          header: `Bạn có chắc chắn không?`,
          width: '400px',
          modal: true,
          data: {
            mode: 'role-delete',
            role: viewRoleDto,
            msg: res !== null ? res.msg : '',
          },
        })
      );

      this.ref()?.onClose.subscribe((res) => {
        if (res != null) {
          if (res !== '') {
            this.roleService.deleteRoleAssign(viewRoleDto.roleId, res).subscribe();
          } else {
            this.roleService.deleteRoleUnAssign(viewRoleDto.roleId).subscribe();
          }
          this.msgService.add({
            severity: 'success',
            summary: 'Thành công',
            detail: `Xoá quyền thành công!`,
          });
          this.roles.update((oRoles) => oRoles.filter((r) => r.roleId !== viewRoleDto.roleId));
        }
      });
    });
  }

  editPermissions(viewRoleDto: ViewRolesDto) {
    this.roleService.getPermissions(viewRoleDto.roleId).subscribe((res: any) => {
      this.permissions.set(res);

      this.ref.set(
        this.dialogService.open(PermissionFormComponent, {
          header: `Quyền ${viewRoleDto.roleName}`,
          width: '400px',
          modal: true,
          data: {
            mode: 'role-permission',
            role: viewRoleDto,
            permissions: this.permissions(),
          },
        })
      );

      this.ref()?.onClose.subscribe((res) => {
        console.log(res);
        if (res) {
          this.roleService.updatePermissions(viewRoleDto.roleId, res).subscribe({
            next: (pers) => {
              this.msgService.add({
                severity: 'success',
                summary: 'Thành công',
                detail: `Phân quyền thành công`,
              });
            },
            error: (err) => {
              this.msgService.add({
                severity: 'error',
                summary: 'Thất bại',
                detail: err,
              });
            },
          });
        }
      });
    });
  }

  setCurrentRole(role: ViewRolesDto, event: Event, menu: any) {
    this.currentRole.set(role);
    this.menuItems.set([
      {
        label: 'Sửa',
        command: () => this.editRole(role),
      },
      {
        label: 'Phân quyền',
        command: () => this.editPermissions(role),
      },
      {
        label: 'Lịch sử thay đổi',
      },
      {
        label: 'Xoá',
        command: () => this.deleteRole(role),
      },
    ]);
    menu.toggle(event);
  }

  editRole(viewRoleDto: ViewRolesDto) {
    this.ref.set(
      this.dialogService.open(RoleFormComponent, {
        header: 'Sửa quyền',
        width: '400px',
        modal: true,
        data: { mode: 'edit', role: viewRoleDto },
      })
    );

    this.ref()?.onClose.subscribe((res) => {
      if (res) {
        const dtoRole: RoleDto = { roleName: res };
        this.roleService.editRole(viewRoleDto.roleId, dtoRole).subscribe({
          next: (resViewDto) => {
            this.roles.update((oRoles) =>
              oRoles.map((r) => (r.roleId === resViewDto.roleId ? resViewDto : r))
            );
            this.msgService.add({
              severity: 'success',
              summary: 'Thành công',
              detail: `Sửa quyền thành công`,
            });
          },
          error: (err) => {
            this.msgService.add({
              severity: 'error',
              summary: 'Thất bại',
              detail: `Quyền ${res} đã tồn tại`,
            });
          },
        });
      }
    });
  }

  newRole() {
    this.ref.set(
      this.dialogService.open(RoleFormComponent, {
        header: 'Thêm quyền',
        width: '400px',
        modal: true,
        data: { mode: 'add' },
      })
    );

    this.ref()?.onClose.subscribe((res) => {
      if (res) {
        const dtoRole: RoleDto = { roleName: res };
        this.roleService.addRole(dtoRole).subscribe({
          next: (resViewDto) => {
            this.roles.update((oRoles) => [...oRoles, resViewDto]);
            this.msgService.add({
              severity: 'success',
              summary: 'Thành công',
              detail: `Thêm quyền ${res} thành công`,
            });
          },
          error: (err) => {
            this.msgService.add({
              severity: 'error',
              summary: 'Thất bại',
              detail: `Quyền ${res} đã tồn tại`,
            });
          },
        });
      }
    });
  }
}
