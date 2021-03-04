import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TutoringApplication } from 'src/app/models/tutoring/tutoring-application';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class TutoringApplicationsService {
  private controller = 'TutoringApplications';

  constructor(
    private httpService: HttpService
  ) { }

  public getTutoringApplications(): Observable<TutoringApplication[]> {
    return this.httpService.get(this.controller, '');
  }

  public confirmTutoringApplication(id: number): Observable<any> {
    return this.httpService.post(this.controller, `${id}/confirm`, null);
  }

  public rejectTutoringApplication(id: number): Observable<any> {
    return this.httpService.post(this.controller, `${id}/reject`, null);
  }
}
