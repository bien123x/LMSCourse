import { join } from 'node:path';
import { CommonModule, DatePipe } from '@angular/common';
import { Component, computed, inject, OnDestroy, OnInit, signal, Directive } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Menu } from 'primeng/menu';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { LockEndTimeDto, ResetPasswordDto, ViewUserDto } from '../../../core/models/user-model';
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
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SelectModule } from 'primeng/select';
import { DatePicker } from 'primeng/datepicker';
import { PasswordValidatorDirective } from '../../../core/validators/password-validator.directive';
import { ViewRolesDto } from '../../../core/models/role-model';
import { RoleService } from '../../../core/services/role.service';

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
    IconFieldModule,
    InputIconModule,
    SelectModule,
    DatePicker,
    PasswordValidatorDirective,
  ],
  providers: [MessageService, DialogService],
})
export class UsersComponent implements OnInit, OnDestroy {
  private dialogSerive = inject(DialogService);
  private msgService = inject(MessageService);
  private userService = inject(UserService);
  private roleService = inject(RoleService);
  getUserApi = new Subscription();

  users = signal<ViewUserDto[]>([]);
  totalRecords = signal<number>(0);
  pageSize = signal<number>(4);
  loading = false;
  currentUser = signal<ViewUserDto | null>(null);

  ref = signal<DynamicDialogRef | undefined>(undefined);

  menuItems = signal<MenuItem[]>([]);

  visibleResetPwd = false;
  resetPwd: ResetPasswordDto = { passwordHash: '' };

  visibleLockUser = false;
  lockEndTimeDto: LockEndTimeDto = { lockEndtime: null };

  roles: ViewRolesDto[] = [];

  ngOnInit(): void {
    this.loadUsers({
      first: 0,
      rows: 4,
    });
    this.lockEndTimeDto = { lockEndtime: new Date() };

    this.roleService.getRoles().subscribe((roles) => {
      this.roles = roles;
    });
  }

  ngOnDestroy(): void {
    this.getUserApi.unsubscribe();
  }

  loadUsers(event: any) {
    console.log('Event:', event);
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
        res.items.forEach((u) => {
          u.creationTime = new Date(u.creationTime + 'Z');
          u.modificationTime = new Date(u.modificationTime + 'Z');
          if (u.lockoutEndTime) u.lockoutEndTime = new Date(u.lockoutEndTime + 'Z');
        });
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

      ...(this.isUserLocked(viewUser)
        ? [
            {
              label: 'Khoá tài khoản',
              command: () => this.clickVisibleLockEndTime(viewUser),
            },
            {
              label: 'Gỡ khoá',
              command: () => this.unlockEndTimeUser(viewUser),
            },
          ]
        : [
            {
              label: 'Khoá tài khoản',
              command: () => this.clickVisibleLockEndTime(viewUser),
            },
          ]),
      {
        label: 'Xoá',
        command: () => this.deleteUser(viewUser),
      },
    ]);
    menu.toggle(event);
  }

  clickVisibleLockEndTime(viewUser: ViewUserDto) {
    this.visibleLockUser = true;
    console.log(viewUser.lockoutEndTime);
    if (!this.isUserLocked(viewUser)) this.lockEndTimeDto.lockEndtime = new Date();
    else this.lockEndTimeDto.lockEndtime = new Date(viewUser.lockoutEndTime + 'Z');
  }

  isUserLocked(user: ViewUserDto): boolean {
    if (!user?.lockoutEndTime) return false;
    const lockoutEnd = new Date(user.lockoutEndTime + 'Z');
    return lockoutEnd > new Date();
  }

  unlockEndTimeUser(viewUser: ViewUserDto) {
    this.userService.unlockEndTimeApi(viewUser.userId).subscribe({
      next: (result) => {
        this.msgService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: result.data,
        });
        this.loadUsers({
          first: 0,
          rows: 4,
        });
      },
      error: (err) => {
        this.msgService.add({
          severity: 'error',
          detail: err.error,
        });
      },
    });
  }

  lockoutUser(viewUser: ViewUserDto) {
    console.log('Save:', this.lockEndTimeDto);
    this.userService.lockEndTimeApi(viewUser.userId, this.lockEndTimeDto).subscribe({
      next: (result) => {
        this.msgService.add({
          severity: 'success',
          summary: 'Khoá thành công',
          detail: result.message + ' đến ' + new Date(result.data).toLocaleString(),
        });
        this.loadUsers({
          first: 0,
          rows: 4,
        });
      },
      error: (err) => {
        console.log(err);
        this.msgService.add({
          severity: 'error',
          detail: err.error,
        });
      },
    });
    this.visibleLockUser = false;
  }

  log(value: any) {
    console.log(value);
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
            this.loadUsers({
              first: 0,
              rows: 4,
            });
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
    this.userService.resetPassword(viewUser.userId, this.resetPwd).subscribe({
      next: () => {},
      error: (err) => {
        console.log(err);
      },
    });
    this.visibleResetPwd = false;
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
              this.msgService.add({
                severity: 'error',
                summary: 'Thất bại',
                detail: err.error.message,
              });
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
              this.loadUsers({
                first: 0,
                rows: 4,
              });
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
