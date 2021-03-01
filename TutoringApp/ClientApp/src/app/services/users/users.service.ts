import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserUnconfirmed } from 'src/app/models/users/user-unconfirmed';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private usersController = 'Users';

  constructor(
    private httpService: HttpService
  ) { }

  //#region Admin side
  public getUnconfirmedUsers(): Observable<UserUnconfirmed[]> {
    return this.httpService.get(this.usersController, 'unconfirmed');
  }

  public confirmUser(id: string): Observable<any> {
    return this.httpService.post(this.usersController, `${id}/confirm`, null);
  }

  public rejectUser(id: string, rejectionReason: string): Observable<any> {
    return this.httpService.post(this.usersController, `${id}/reject`, rejectionReason);
  }
  //#endregion
}
