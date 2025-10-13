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
    PopoverModule,
    BadgeModule,
    OverlayBadgeModule,
    HasRoleDirective,
    RouterLink,
  ],
})
export class TopbarComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  userLogin = signal<ViewUserDto | undefined>(undefined);
  private msgService = inject(MessageService);
  private cartService = inject(CartService);

  topbarItems!: MenuItem[];

  badgeValue = computed(() => this.cartService.cart().length);

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
      },
      error: (err) => {},
    });
  }

  myAccount() {
    this.router.navigate(['/my-account']);
  }

  logout() {
    this.authService.logout();
    this.msgService.add({
      severity: 'success',
      summary: 'Thành công',
      detail: `Đăng xuất tài khoản ${this.userLogin()?.userName}`,
      icon: 'pi pi-user',
      life: 3000,
    });
    this.router.navigate(['/auth/login']);
  }
}
