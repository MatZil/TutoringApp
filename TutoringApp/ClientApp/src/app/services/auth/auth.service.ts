import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserRegistration } from 'src/app/models/auth/user-registration';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authController = 'Authentication';

  constructor(
    private httpService: HttpService
  ) { }

  public register(userRegistration: UserRegistration): Observable<any> {
    return this.httpService.post(this.authController, 'register', userRegistration);
  }
}
