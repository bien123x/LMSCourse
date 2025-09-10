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

  setCurrentUser(user: ViewUserDto, event: Event, menu: any) {
    this.menuItems.set([
      {
        label: 'Xem chi tiết',
      },
      {
        label: 'Sửa',
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
  newUser() {
    // this.ref = this.dialogSerive.open()
  }
}
