import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TutoringSession } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session';
import { TutoringSessionEvaluation } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-evaluation';
import { TutoringSessionNew } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-new';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class TutoringSessionsService {
  private sessionsController = 'TutoringSessions';

  constructor(
    private httpService: HttpService
  ) { }

  public getTutoringSessions(): Observable<TutoringSession[]> {
    return this.httpService.get(this.sessionsController, '');
  }

  public getLearningSessions(): Observable<TutoringSession[]> {
    return this.httpService.get(this.sessionsController, 'learning');
  }

  public createTutoringSession(tutoringSessionNew: TutoringSessionNew): Observable<any> {
    return this.httpService.post(this.sessionsController, '', tutoringSessionNew);
  }

  public cancelTutoringSession(sessionId: number): Observable<any> {
    return this.httpService.put(this.sessionsController, sessionId.toString(), null);
  }

  public invertTutoringSession(sessionId: number): Observable<any> {
    return this.httpService.put(this.sessionsController, `${sessionId}/invert`, null);
  }

  public evaluateTutoringSession(sessionId: number, evaluation: TutoringSessionEvaluation): Observable<any> {
    return this.httpService.post(this.sessionsController, `${sessionId}/evaluate`, evaluation);
  }
}
