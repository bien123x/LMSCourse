import { join } from 'node:path';
import { CommonModule, DatePipe } from '@angular/common';
import { Component, computed, inject, OnDestroy, OnInit, signal, Directive } from '@angular/core';
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
import { FilterField, QueryDto, SortField } from '../../../core/models/query-model';
import { Subscription } from 'rxjs';
import { HasPermissionDirective } from '../../../core/directives/has-permission-directive';

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
    HasPermissionDirective,
  ],
  providers: [MessageService, DialogService],
})
export class UsersComponent implements OnInit, OnDestroy {
  private dialogSerive = inject(DialogService);
  private msgService = inject(MessageService);
  private userService = inject(UserService);
  getUserApi = new Subscription();

  users = signal<ViewUserDto[]>([]);
  totalRecords = signal<number>(0);
  pageSize = signal<number>(4);
  loading = false;
  currentUser = signal<ViewUserDto | undefined>(undefined);

  ref = signal<DynamicDialogRef | undefined>(undefined);

  menuItems = signal<MenuItem[]>([]);

  visibleResetPwd = false;
  resetPwd: ResetPasswordDto = { passwordHash: '' };

  ngOnInit(): void {
    // this.userService.getViewUsers().subscribe((res: any) => {
    //   this.users.set(res);
    //   console.log(this.users());
    // });
    this.loadUsers({
      first: 0,
      rows: 4,
    });
  }

  ngOnDestroy(): void {
    this.getUserApi.unsubscribe();
  }

  loadUsers(event: any) {
    // console.log('Event:', event);
    const pageNumber = event?.first != null && event?.rows ? event.first / event.rows + 1 : 1;
    this.pageSize.set(event.rows);
    this.loading = true;
    let sorts: SortField[] = [];
    if (event.multiSortMeta) {
      sorts = event.multiSortMeta.map((s: any) => ({
        field: s.field,
        order: s.order === 1 ? 'asc' : 'desc',
      }));
    }
    // console.log(sorts);
    // // filters
    const filters: FilterField[] = [];
    if (event.filters && Object.keys(event.filters).length > 0) {
      for (const key of Object.keys(event.filters)) {
        const f = event.filters[key];
        if (f && f.value) {
          filters.push({ field: key, value: f.value });
        }
      }
    }
    const query: QueryDto = {
      pageNumber: pageNumber,
      pageSize: this.pageSize(),
      sorts: sorts,
      filters: filters,
    };
    // console.log('Query:', query);
    this.getUserApi = this.userService.getViewUsersPagination(query).subscribe({
      next: (res) => {
        console.log(res);
        this.users.set(res.items);
        this.totalRecords.set(res.totalCount);
        this.loading = false;
      },
      error: (err) => {
        console.log('Request thất bại, nhưng không hiển thị 401 trên console');
      },
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
              detail: `Xoá người dùng thành công`,
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
    this.visibleResetPwd = true;
  }

  resetPassword(viewUser: ViewUserDto) {
    console.log('vao');
    console.log(this.resetPwd);
    this.userService.resetPassword(viewUser.userId, this.resetPwd).subscribe({
      next: () => {
        this.visibleResetPwd = false;
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
          width: '400px',
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
              if (editUserDto.isActive === false) {
                this.users.update((oUsers) =>
                  oUsers.filter((u) => u.userId !== editUserDto.userId)
                );
                this.totalRecords.update((oTotal) => oTotal - 1);
              }
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
              if (err.error.errors) {
                this.msgService.add({
                  severity: 'error',
                  summary: 'Lỗi',
                  detail: err.error.errors.join('\n'),
                });
              } else if (err.error) {
                this.msgService.add({
                  severity: 'error',
                  summary: 'Lỗi',
                  detail: err.error,
                });
              }
            },
          });
        }
      });
    });
  }
}
