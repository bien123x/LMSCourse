import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { filter } from 'rxjs';
import { Toast } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageModule } from 'primeng/message';
import { TopbarComponent } from './shared/layout/topbar/topbar.component';
import { SidebarComponent } from './shared/layout/sidebar/sidebar.component';
import { BreadcrumbComponent } from './shared/breadcrumb/breadcrumb.component';
import { AuthService } from './core/services/auth.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    ButtonModule,
    Toast,
    ConfirmDialogModule,
    MessageModule,
    TopbarComponent,
    SidebarComponent,
    BreadcrumbComponent,
  ],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  protected readonly title = signal('Khoá học trực tuyến');

  private router = inject(Router);
  private authService = inject(AuthService);
  private msgService = inject(MessageService);

  isAuthPage = signal<boolean>(false);
  items = computed(() => this.authService.menuItems());

  ngOnInit(): void {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.isAuthPage.set(event.urlAfterRedirects.startsWith('/auth'));
      });
  }

  onClose() {
    
  }
}
