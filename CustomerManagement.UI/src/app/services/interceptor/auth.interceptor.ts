import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  let cookieService = inject(CookieService);

  const token = cookieService.get('loginToken');
  if (token) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: 'Bearer ' + token,
      },
    });
    return next(authReq);
  } else {
    return next(req);
  }
};
