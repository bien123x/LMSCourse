import { Routes, CanActivate } from '@angular/router';
import { LoginComponent } from './features/auth/login/login';
import { AuthComponent } from './features/auth/auth.component';
import { RegisterComponent } from './features/auth/register/register';
import { AuthGuard } from './core/guards/auth-guard';
import { RoleGuard } from './core/guards/role-guard';
import { AuditLogsComponent } from './features/admin/audit-logs/audit-logs';
import { RolesComponent } from './features/admin/identity/roles/roles';
import { UsersComponent } from './features/admin/identity/users/users';
import { IdentityComponent } from './features/admin/identity/identity';
import { SettingsComponent } from './features/admin/settings/settings';
import { CourseComponent } from './features/student/course/course.component';
import { CourseDetailComponent } from './features/student/course/course-detail/course-detail.component';
import { CartComponent } from './features/student/course/cart/cart.component';
import { HomeComponent } from './features/admin/home/home';
import { CheckoutComponent } from './features/student/checkout/checkout.component';
import { PaymentResultComponent } from './features/student/payment-result/payment-result.component';
import { CourseEnrolledComponent } from './features/student/course-enrolled/course-enrolled.component';
import { CourseWatchComponent } from './features/student/course-watch/couse-watch.component';
import { QuizQuestionsComponent } from './features/student/quiz-questions/quiz-questions.component';
import { DashboardStudentComponent } from './features/student/dashboard-student/dashboard-student.component';
import { StudentCertificatesComponent } from './features/student/student-certificates/student-certificates.component';
import { MyAccountComponent } from './features/my-account/my-account.component';

export const routes: Routes = [
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ],
  },
  {
    path: 'admin',
    children: [
      { path: '', redirectTo: 'identity/users', pathMatch: 'full' },
      {
        path: 'identity',
        component: IdentityComponent,
        children: [
          { path: 'roles', component: RolesComponent },
          { path: 'users', component: UsersComponent },
        ],
      },
      {
        path: 'settings',
        component: SettingsComponent,
      },
      { path: 'audit-logs', component: AuditLogsComponent },
    ],
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin'] },
  },
  {
    path: 'student',
    children: [
      { path: '', redirectTo: 'courses', pathMatch: 'full' },
      { path: 'courses', component: CourseComponent },
      { path: 'courses/detail/:id', component: CourseDetailComponent },
      { path: 'cart', component: CartComponent },
      { path: 'checkout', component: CheckoutComponent },
      { path: 'payment-result', component: PaymentResultComponent },
      { path: 'courses-enrolled', component: CourseEnrolledComponent },
      { path: 'courses-enrolled/:id', component: CourseWatchComponent },
      { path: 'quiz-question/:id', component: QuizQuestionsComponent },
      { path: 'dashboard', component: DashboardStudentComponent },
      { path: 'certificates', component: StudentCertificatesComponent },
    ],
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Student'] },
  },
  { path: 'my-account', component: MyAccountComponent },
  { path: 'home', component: HomeComponent },

  { path: '**', redirectTo: 'home' },
];
