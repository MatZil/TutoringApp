import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { UserLogin } from 'src/app/models/auth/user-login';
import { AuthService } from 'src/app/services/auth/auth.service';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public isLoggedIn: Observable<boolean>;

  constructor(
    private authService: AuthService,
    private httpService: HttpService
  ) { }

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isAuthenticated$;
  }

  public login(): void {
    const userLogin: UserLogin = {
      email: 'zilinskas.matas1999@gmail.com',
      password: 'sixlet'
    };

    this.authService.login(userLogin).subscribe();
  }

  public logout(): void {
    this.authService.logout();
  }

  public mockRequest(): void {
    this.httpService.get('Authentication', '').subscribe();
  }
}
