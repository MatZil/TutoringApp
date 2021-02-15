import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginResponse } from 'src/app/models/auth/login-response';
import { UserLogin } from 'src/app/models/auth/user-login';
import { UserRegistration } from 'src/app/models/auth/user-registration';
import { HttpService } from '../http.service';
import { AppConstants } from 'src/app/app.constants';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authController = 'Authentication';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(
    private httpService: HttpService
  ) { }

  public register(userRegistration: UserRegistration): Observable<any> {
    return this.httpService.post(this.authController, 'register', userRegistration);
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

  private changeAuthState(isAuthenticated: boolean): void {
    if (this.isAuthenticatedSubject.value !== isAuthenticated) {
      this.isAuthenticatedSubject.next(isAuthenticated);
    }
  }
}
