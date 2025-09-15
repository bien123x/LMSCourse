import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem, TreeNode } from 'primeng/api';
import { DrawerModule } from 'primeng/drawer';
import { ButtonModule } from 'primeng/button';
import { PanelMenu } from 'primeng/panelmenu';

@Component({
  selector: 'app-left-sidebar',
  templateUrl: './left-sidebar.html',
  imports: [PanelMenu, DrawerModule, ButtonModule],
})
export class LeftSidebarComponent implements OnInit {
  private router = inject(Router);
  items: MenuItem[] = [];

  ngOnInit(): void {
    this.items = [
      {
        label: 'Trang chủ',
        icon: 'pi pi-home',
        command: () => this.router.navigate(['/home']),
      },
      {
        label: 'Dashboard',
        icon: 'pi pi-chart-line',
        command: () => this.router.navigate(['/dashboard']),
      },
      {
        label: 'Quản trị',
        icon: 'pi pi-cog',
        items: [
          {
            label: 'Quản lý tài khoản',
            icon: 'pi pi-users',
            items: [
              {
                label: 'Quyền',
                icon: 'pi pi-lock',
                command: () => this.router.navigate(['/identity/roles']),
              },
              {
                label: 'Người dùng',
                icon: 'pi pi-user',
                command: () => this.router.navigate(['/identity/users']),
              },
            ],
          },
          {
            label: 'Nhật ký',
            icon: 'pi pi-book',
            command: () => this.router.navigate(['/logs']),
          },
          {
            label: 'Cài đặt',
            icon: 'pi pi-sliders-h',
            command: () => this.router.navigate(['/settings']),
          },
        ],
      },
    ];
  }
}
