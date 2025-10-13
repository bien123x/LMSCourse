import { AfterViewInit, Component, inject, input, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DrawerModule } from 'primeng/drawer';
import { BreakpointObserver, Breakpoints, LayoutModule } from '@angular/cdk/layout';
import { PanelMenuModule } from 'primeng/panelmenu';
import { MenuItem } from 'primeng/api';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { CodePermission } from '../../../core/models/constant';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  imports: [ButtonModule, DrawerModule, LayoutModule, PanelMenuModule],
})
export class SidebarComponent implements OnInit {
  private breakpointObserver = inject(BreakpointObserver);

  visible: boolean = false;
  iconDrawer = signal<string>('');

  // items: MenuItem[] = [];
  items = input<MenuItem[]>([]);
  ngOnInit(): void {
    this.breakpointObserver.observe([Breakpoints.Large]).subscribe((result) => {});
  }
}
