import { PERMISSION } from './../../../core/models/constant';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MenuItem, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { Menu, MenuModule } from 'primeng/menu';
import { TableModule } from 'primeng/table';
import { AuthService } from '../../../core/services/auth.service';
import { ViewUserDto } from '../../../core/models/user-model';
import { Avatar } from 'primeng/avatar';
import { PopoverModule } from 'primeng/popover';
import { BadgeModule } from 'primeng/badge';
import { OverlayBadgeModule } from 'primeng/overlaybadge';
import { HasRoleDirective } from '../../../core/directives/has-role-directive';
import { CartService } from '../../../core/services/cart.service';
import { HasPermissionDirective } from '../../../core/directives/has-permission-directive';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.css',
  imports: [
    ButtonModule,
    RouterLink,
    Menu,
    TableModule,
    MenuModule,
    Avatar,
    TieredMenuModule,
    PopoverModule,
    BadgeModule,
    OverlayBadgeModule,
    RouterLink,
    HasPermissionDirective,
    HasRoleDirective,
    CommonModule,
  ],
})
export class TopbarComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  userLogin = signal<ViewUserDto | undefined>(undefined);
  private msgService = inject(MessageService);
  private cartService = inject(CartService);

  topbarItems!: MenuItem[];

  changeRoleItems!: MenuItem[];

  currentRole = computed(() => this.authService.getCurrentRole());

  badgeValue = computed(() => this.cartService.cart().length);

  PERMISSION = PERMISSION;

  constructor() {}
  ngOnInit(): void {
    this.authService.me().subscribe({
      next: (user) => {
        this.userLogin.set(user);

        this.topbarItems = [
          {
            label: 'Cài đặt của tôi',
            icon: 'pi pi-user',
            command: () => this.myAccount(),
          },
          {
            label: 'Logout',
            icon: 'pi pi-sign-out',
            command: () => this.logout(),
          },
        ];

        this.changeRoleItems = [
          {
            label: 'Admin',
            visible: this.authService.hasRole('Admin'),
            routerLink: '/admin',
            command: () => this.authService.switchRole('Admin'),
          },
          {
            label: 'Teacher',
            visible: this.authService.hasRole('Teacher'),
            routerLink: '/teacher',
            command: () => this.authService.switchRole('Teacher'),
          },
          {
            label: 'Student',
            visible: this.authService.hasRole('Student'),
            routerLink: '/student',
            command: () => this.authService.switchRole('Student'),
          },
        ];
      },
      error: (err) => {},
    });
  }

  get isHasManyRoles() {
    return this.authService.isHasManyRoles();
  }

  myAccount() {
    this.router.navigate(['/my-account']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
