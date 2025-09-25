import { Routes, CanActivate } from '@angular/router';
import { HomeComponent } from './features/home/home';
import { LoginComponent } from './features/auth/login/login';
import { AuthComponent } from './features/auth/auth.component';
import { RegisterComponent } from './features/auth/register/register';
import { App } from './app';
import { IdentityComponent } from './features/identity/identity';
import { RolesComponent } from './features/identity/roles/roles';
import { UsersComponent } from './features/identity/users/users';
import { SettingsComponent } from './features/settings/settings';
import { AuthGuard } from './core/guards/auth-guard';
import { RoleGuard } from './core/guards/role-guard';
import { AuditLogsComponent } from './features/audit-logs/audit-logs';
import { CourseComponent } from './features/course/course.component';
import { CourseDetailComponent } from './features/course/course-detail/course-detail.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },

  { path: 'home', component: HomeComponent },
  {
    path: 'identity',
    component: IdentityComponent,
    children: [
      { path: 'roles', component: RolesComponent },
      { path: 'users', component: UsersComponent },
    ],
    canActivate: [RoleGuard],
    data: { roles: ['Admin'] },
  },
  {
    path: 'courses',
    component: CourseComponent,
  },
  { path: 'courses/detail/:id', component: CourseDetailComponent },
  {
    path: 'settings',
    component: SettingsComponent,
    // canActivate: [AuthGuard, RoleGuard],
    // data: { roles: ['Admin'] },
  },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ],
  },
  { path: 'audit-logs', component: AuditLogsComponent },
  { path: '**', redirectTo: 'home' },
];
