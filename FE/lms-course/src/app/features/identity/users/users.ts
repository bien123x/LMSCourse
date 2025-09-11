import { CommonModule, DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Menu } from 'primeng/menu';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ResetPasswordDto, ViewUserDto } from '../../../core/models/user-model';
import { UserService } from '../../../core/services/user.service';
import { UserFormComponent } from '../../../shared/user-form/user-form';
import { PermissionFormComponent } from '../../../shared/permission-form/permission-form';
import { DialogModule } from 'primeng/dialog';
import { IftaLabel } from 'primeng/iftalabel';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { DeleteConfirmComponent } from '../../../shared/delete-confirm/delete-confirm';

@Component({
  selector: 'app-users',
  templateUrl: './users.html',
  imports: [
    ButtonModule,
    TableModule,
    IftaLabel,
    CommonModule,
    Menu,
    ToastModule,
    DatePipe,
    DialogModule,
    FormsModule,
    InputTextModule,
    PasswordModule,
  ],
  providers: [MessageService, DialogService],
})
export class UsersComponent implements OnInit {
  private dialogSerive = inject(DialogService);
  private msgService = inject(MessageService);
  private userService = inject(UserService);

  users = signal<ViewUserDto[]>([]);
  totalRecords = signal<number>(0);
  pageSize = signal<number>(2);
  currentUser = signal<ViewUserDto | undefined>(undefined);

  ref = signal<DynamicDialogRef | undefined>(undefined);
  totalUsers = computed<number>(() => this.users().length);

  menuItems = signal<MenuItem[]>([]);

  visibleResetPwd = signal<boolean>(false);
  resetPwd: ResetPasswordDto = { passwordHash: '' };

  ngOnInit(): void {
    // this.userService.getViewUsers().subscribe((res: any) => {
    //   this.users.set(res);
    //   console.log(this.users());
    // });
    this.loadUsers({ first: 0, rows: this.pageSize });
  }

  loadUsers(event: any) {
    const pageNumber = event.first / event.rows + 1;
    const pageSize = event.rows;

    this.userService.getViewUsersPagination(pageNumber, pageSize).subscribe((res) => {
      this.users.set(res.items);
      this.totalRecords.set(res.totalCount);
    });
  }

  setCurrentUser(viewUser: ViewUserDto, event: Event, menu: any) {
    this.currentUser.set(viewUser);
    this.menuItems.set([
      {
        label: 'Xem chi tiết',
        command: () => this.viewDetail(viewUser),
      },
      {
        label: 'Sửa',
        command: () => this.editUser(viewUser),
      },
      {
        label: 'Phân quyền',
        command: () => this.userPermissions(viewUser),
      },
      {
        label: 'Lịch sử thay đổi',
      },
      {
        label: 'Thiết đặt mật khẩu',
        command: () => this.clickVisibleResetPwd(),
      },
      {
        label: 'Xoá',
        command: () => this.deleteUser(viewUser),
      },
    ]);
    menu.toggle(event);
  }

  deleteUser(viewUser: ViewUserDto) {
    this.ref.set(
      this.dialogSerive.open(DeleteConfirmComponent, {
        header: 'Bạn có chắc muốn xoá người dùng này?',
        width: 'auto',
        modal: true,
        data: {
          mode: 'user-delete',
          user: viewUser,
          msg: `Xoá người dùng ${viewUser.userName}`,
        },
      })
    );

    this.ref()?.onClose.subscribe((res) => {
      if (res != null) {
        this.userService.deleteUser(viewUser.userId).subscribe({
          next: () => {
            this.msgService.add({
              severity: 'success',
              summary: 'Thành công',
              detail: `Phân quyền thành công`,
            });
            this.users.update((oUsers) => oUsers.filter((u) => u.userId !== viewUser.userId));
          },
          error: (err) => {
            console.log(err);
          },
        });
      }
    });
  }

  clickVisibleResetPwd() {
    this.resetPwd.passwordHash = '';
    this.visibleResetPwd.set(true);
  }

  resetPassword(viewUser: ViewUserDto) {
    console.log('vao');
    console.log(this.resetPwd);
    this.userService.resetPassword(viewUser.userId, this.resetPwd).subscribe({
      next: () => {
        this.visibleResetPwd.set(false);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  userPermissions(viewUser: ViewUserDto) {
    this.userService.getUserPermissions(viewUser.userId).subscribe((uPerms) => {
      this.ref.set(
        this.dialogSerive.open(PermissionFormComponent, {
          header: 'Phân quyền người dùng',
          width: 'auto',
          modal: true,
          data: {
            mode: 'user-permission',
            userPermissions: uPerms.userPermissions,
            rolePermissions: uPerms.rolePermissions,
          },
        })
      );

      this.ref()?.onClose.subscribe((res) => {
        if (res)
          this.userService.updateUserPermissions(viewUser.userId, res).subscribe({
            next: (perms) => {
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
      });
    });
  }

  viewDetail(viewUser: ViewUserDto) {
    this.userService.getRolesName().subscribe((rolesName) => {
      this.ref.set(
        this.dialogSerive.open(UserFormComponent, {
          header: 'Chi tiết người dùng',
          width: 'auto',
          modal: true,
          data: { mode: 'viewDetail', rolesName: rolesName, viewUser: viewUser },
        })
      );
    });
    this.ref()?.onClose.subscribe((res) => {});
  }

  editUser(viewUser: ViewUserDto) {
    this.userService.getRolesName().subscribe((rolesName) => {
      this.ref.set(
        this.dialogSerive.open(UserFormComponent, {
          header: 'Sửa người dùng',
          width: 'auto',
          modal: true,
          data: { mode: 'edit', rolesName: rolesName, viewUser: viewUser },
        })
      );

      this.ref()?.onClose.subscribe((res) => {
        console.log(res);
        if (res !== undefined) {
          this.userService.editUser(viewUser.userId, res).subscribe({
            next: (editUserDto) => {
              this.users.update((oUsers) =>
                oUsers.map((u) => (u.userId === viewUser.userId ? editUserDto : u))
              );
            },
            error: (err) => {
              console.log(err);
            },
          });
        }
      });
    });
  }

  newUser() {
    this.userService.getRolesName().subscribe((res) => {
      this.ref.set(
        this.dialogSerive.open(UserFormComponent, {
          header: 'Thêm người dùng',
          width: 'auto',
          modal: true,
          data: { mode: 'add', rolesName: res },
        })
      );

      this.ref()?.onClose.subscribe((res) => {
        if (res !== undefined) {
          this.userService.addUser(res).subscribe({
            next: (viewUserDto) => {
              this.users.update((oUsers) => [...oUsers, viewUserDto]);
            },
            error: (err) => {
              console.log(err);
            },
          });
          console.log(res);
        }
      });
    });
  }
}
