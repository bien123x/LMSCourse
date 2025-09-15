import { Component, effect, inject, OnInit, signal, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../core/services/auth.service';
import { AvatarModule } from 'primeng/avatar';
import { Popover } from 'primeng/popover';
import { MenuItem } from 'primeng/api';
import { Menu } from 'primeng/menu';

@Component({
  selector: 'app-right-sidebar',
  templateUrl: './right-sidebar.html',
  imports: [ButtonModule, AvatarModule, Menu],
})
export class RightSidebarComponent implements OnInit {
  private router = inject(Router);
  public authService = inject(AuthService);
  user = signal({ userName: '' });
  items = signal<MenuItem[] | undefined>(undefined);
  ngOnInit(): void {
    if (this.isLoggedIn()) {
      this.authService.loadCurrentUser().subscribe((res) => {
        this.user.set(res);
      });
    }
    this.items.set([
      {
        label: 'Tài khoản của tôi',
        icon: 'pi pi-refresh',
      },
      {
        label: 'Đăng xuất',
        icon: 'pi pi-upload',
        command: () => this.logout(),
      },
    ]);
  }

  goToLogin() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }
}
