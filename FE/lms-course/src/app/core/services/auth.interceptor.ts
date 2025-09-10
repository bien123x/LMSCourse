import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { error } from 'console';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { RefreshRequestDto } from '../models/auth-model';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = typeof localStorage !== 'undefined' ? localStorage.getItem('access_token') : null;
  const refresh =
    typeof localStorage !== 'undefined' ? localStorage.getItem('refresh_token') : null;
  const router = inject(Router);
  const authService = inject(AuthService);

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && refresh) {
        const refreshToken: RefreshRequestDto = { refreshToken: refresh };
        return authService.refreshToken(refreshToken).pipe(
          switchMap((res: any) => {
            console.log(res);
            localStorage.setItem('access_token', res.accessToken);
            localStorage.setItem('refresh_token', res.refreshToken);

            // gửi lại request ban đầu với access token mới
            const retryReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${res.accessToken}`,
              },
            });
            return next(retryReq);
          }),
          catchError((err) => {
            // refresh cũng fail → logout
            localStorage.removeItem('access_token');
            localStorage.removeItem('refresh_token');
            router.navigate(['/auth/login']);
            return throwError(() => err);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
