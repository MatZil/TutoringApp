import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AppConstants } from './app.constants';
import { AuthService } from './services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
    Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const isCurrentlyAuthenticated = this.authService.isCurrentlyAuthenticated();

    if (isCurrentlyAuthenticated) {
      return true;
    }

    this.router.navigate([AppConstants.LoginRoute], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
