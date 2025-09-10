import { Component, inject, OnInit, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RightSidebarComponent } from './shared/right-sidebar/right-sidebar';
import { LeftSidebarComponent } from './shared/left-sidebar/left-sidebar';
import { AuthService } from './core/services/auth.service';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ButtonModule, RightSidebarComponent, LeftSidebarComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  protected readonly title = signal('lms-course');

  private authService = inject(AuthService);
  private router = inject(Router);

  isAuthPage = signal<boolean>(false);

  ngOnInit(): void {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.isAuthPage.set(event.urlAfterRedirects.startsWith('/auth'));
      });
  }
}
