import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Menu } from 'primeng/menu';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ViewUserDto } from '../../../core/models/user-model';
import { UserService } from '../../../core/services/user.service';
import { UserFormComponent } from '../../../shared/user-form/user-form';

@Component({
  selector: 'app-users',
  templateUrl: './users.html',
  imports: [ButtonModule, TableModule, CommonModule, Menu, ToastModule],
  providers: [MessageService, DialogService],
})
export class UsersComponent implements OnInit {
  private dialogSerive = inject(DialogService);
  private msgService = inject(MessageService);
  private userService = inject(UserService);

  users = signal<ViewUserDto[]>([]);

  ref = signal<DynamicDialogRef | undefined>(undefined);
  totalUsers = computed<number>(() => this.users().length);

  menuItems = signal<MenuItem[]>([]);

  ngOnInit(): void {
    this.userService.getViewUsers().subscribe((res: any) => this.users.set(res));
  }

  setCurrentUser(viewUser: ViewUserDto, event: Event, menu: any) {
    this.menuItems.set([
      {
        label: 'Xem chi tiết',
      },
      {
        label: 'Sửa',
        command: () => this.editUser(viewUser),
      },
      {
        label: 'Phân quyền',
      },
      {
        label: 'Lịch sử thay đổi',
      },
      {
        label: 'Thiết đặt mật khẩu',
      },
      {
        label: 'Xoá',
      },
    ]);
    menu.toggle(event);
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
