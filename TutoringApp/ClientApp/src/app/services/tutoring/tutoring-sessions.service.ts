import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { TutoringSession } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session';
import { TutoringSessionCancel } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-cancel';
import { TutoringSessionEvaluation } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-evaluation';
import { TutoringSessionFinishedNotification } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-finished-notification';
import { TutoringSessionNew } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-new';
import { TutoringSessionOnGoing } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-on-going';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class TutoringSessionsService {
  private sessionsController = 'TutoringSessions';

  private tutoringSessionFinishedSubject = new BehaviorSubject<TutoringSessionFinishedNotification>(null);

  private onGoingTutoringSessionSubject = new Subject<TutoringSessionOnGoing>();
  public onGoingTutoringSession$ = this.onGoingTutoringSessionSubject.asObservable();

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

  public cancelTutoringSession(sessionId: number, tutoringSessionCancel: TutoringSessionCancel): Observable<any> {
    return this.httpService.post(this.sessionsController, `${sessionId}/cancel`, tutoringSessionCancel);
  }

  public invertTutoringSession(sessionId: number): Observable<any> {
    return this.httpService.put(this.sessionsController, `${sessionId}/invert`, null);
  }

  public evaluateTutoringSession(sessionId: number, evaluation: TutoringSessionEvaluation): Observable<any> {
    return this.httpService.post(this.sessionsController, `${sessionId}/evaluate`, evaluation);
  }

  public finishTutoringSession(notification: TutoringSessionFinishedNotification): void {
    this.tutoringSessionFinishedSubject.next(notification);
  }

  public tutoringSessionFinished(): Observable<TutoringSessionFinishedNotification> {
    return this.tutoringSessionFinishedSubject.asObservable();
  }

  public getOnGoingSession(): Observable<TutoringSessionOnGoing> {
    return this.httpService.get(this.sessionsController, 'on-going');
  }

  public setOngoingSession(session: TutoringSessionOnGoing): void {
    this.onGoingTutoringSessionSubject.next(session);
  }
}
