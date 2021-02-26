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
import { CurrentUser } from 'src/app/models/auth/current-user';
import { TokenGetter } from 'src/app/utils/token-getter';

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
    const token = TokenGetter();

    return token && !this.jwtHelperService.isTokenExpired();
  }

  public getCurrentUser(): CurrentUser {
    const token = TokenGetter();
    if (!token || this.jwtHelperService.isTokenExpired()) { return undefined; }

    const decodedToken = this.jwtHelperService.decodeToken();
    return {
      email: decodedToken[AppConstants.EmailClaimType],
      role: decodedToken[AppConstants.RoleClaimType]
    };
  }

  private changeAuthState(isAuthenticated: boolean): void {
    if (this.isAuthenticatedSubject.value !== isAuthenticated) {
      this.isAuthenticatedSubject.next(isAuthenticated);
    }
  }
}
