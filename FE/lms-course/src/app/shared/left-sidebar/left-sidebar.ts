import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TreeNode } from 'primeng/api';
import { Tree } from 'primeng/tree';

@Component({
  selector: 'app-left-sidebar',
  templateUrl: './left-sidebar.html',
  imports: [Tree],
})
export class LeftSidebarComponent implements OnInit {
  private router = inject(Router);
  files!: TreeNode[];

  ngOnInit(): void {
    this.files = [
      {
        label: 'Trang chủ',
        icon: 'pi pi-fw pi-inbox',
        data: { route: '/home' },
      },
      {
        label: 'Dashboard',
        icon: 'pi pi-fw pi-inbox',
        data: {},
      },
      {
        label: 'Quản trị',
        icon: 'pi pi-fw pi-inbox',
        children: [
          {
            label: 'Quản lý tài khoản',
            icon: 'pi pi-fw pi-inbox',
            children: [
              {
                label: 'Quyền',
                icon: 'pi pi-fw pi-inbox',
                data: { route: '/identity/roles' },
              },
              {
                label: 'Người dùng',
                icon: 'pi pi-fw pi-inbox',
                data: { route: '/identity/users' },
              },
            ],
          },
          {
            label: 'Nhật ký',
            icon: 'pi pi-fw pi-inbox',
            data: {},
          },
          {
            label: 'Cài đặt',
            icon: 'pi pi-fw pi-inbox',
            data: {},
          },
        ],
      },
    ];
  }

  nodeSelect(event: any) {
    const route = event.node.data?.route;
    if (route) {
      this.router.navigate([route]);
    }
  }
}
