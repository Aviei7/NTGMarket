import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { UserService } from '../services/user.service';

export const authGuard: CanActivateFn = (route, state) => {
  const auth = inject(UserService);
  const router = inject(Router);

  if (auth.currentUserSnapshot) {
    return of(true);
  }

  return auth.restoreSession().pipe(
    map(user => user ? true : router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } })),
    catchError(() =>
      of(router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } }))
    )
  );
};
