import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginResponse } from 'src/app/models/auth/login-response';
import { UserLogin } from 'src/app/models/auth/user-login';
import { UserRegistration } from 'src/app/models/auth/user-registration';
import { HttpService } from '../http.service';

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
    return this.httpService.post(this.authController, 'login', userLogin);
  }

  public logout(): void {
    localStorage.removeItem('web_token');
    this.changeAuthState(false);
  }

  public changeAuthState(isAuthenticated: boolean): void {
    if (this.isAuthenticatedSubject.value !== isAuthenticated) {
      this.isAuthenticatedSubject.next(isAuthenticated);
    }
  }
}
