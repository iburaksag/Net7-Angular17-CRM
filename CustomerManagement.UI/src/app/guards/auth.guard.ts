import { CanActivateFn, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { inject } from '@angular/core'
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const router:Router = inject(Router);
  const authService:AuthService = inject(AuthService);

  if (authService.isAuthenticated()) {
      return true;
    } else {
      router.navigate(['/login']);
      return false;
    }
};
