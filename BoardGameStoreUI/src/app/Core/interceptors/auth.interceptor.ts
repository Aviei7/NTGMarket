import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { UserService } from '../services/user.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const userService = inject(UserService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        userService.clearAuthState();

        const isSessionProbe = req.url.includes('/api/Users/me');
        const isAuthPage = router.url.startsWith('/login') || router.url.startsWith('/register');

        if (!isSessionProbe && !isAuthPage) {
          router.navigate(['/login']);
        }
      }

      return throwError(() => error);
    })
  );
};
