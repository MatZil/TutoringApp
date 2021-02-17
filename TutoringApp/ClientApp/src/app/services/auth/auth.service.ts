import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginResponse } from 'src/app/models/auth/login-response';
import { UserLogin } from 'src/app/models/auth/user-login';
import { UserRegistration } from 'src/app/models/auth/user-registration';
import { HttpService } from '../http.service';
import { AppConstants } from 'src/app/app.constants';
import { tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { EmailConfirmation } from 'src/app/models/auth/email-confirmation';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authController = 'Authentication';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(
    private httpService: HttpService,
    private jwtHelperService: JwtHelperService
  ) { }

  public register(userRegistration: UserRegistration): Observable<any> {
    return this.httpService.post(this.authController, 'register', userRegistration);
  }

  public confirmEmail(emailConfirmation: EmailConfirmation): Observable<any> {
    return this.httpService.post(this.authController, 'confirm-email', emailConfirmation);
  }

  public login(userLogin: UserLogin): Observable<LoginResponse> {
    return this.httpService.post(this.authController, 'login', userLogin).pipe(
      tap(response => localStorage.setItem(AppConstants.WebTokenKey, response.webToken)),
      tap(_ => this.changeAuthState(true))
    );
  }

  public logout(): void {
    localStorage.removeItem(AppConstants.WebTokenKey);
    this.changeAuthState(false);
  }

  public isCurrentlyAuthenticated(): boolean {
    const token = localStorage.getItem(AppConstants.WebTokenKey);

    return token && !this.jwtHelperService.isTokenExpired();
  }

  private changeAuthState(isAuthenticated: boolean): void {
    if (this.isAuthenticatedSubject.value !== isAuthenticated) {
      this.isAuthenticatedSubject.next(isAuthenticated);
    }
  }
}
