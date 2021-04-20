import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { filter, map, tap } from 'rxjs/operators';
import { TutoringSessionEvaluation } from './models/tutoring/tutoring-sessions/tutoring-session-evaluation';
import { TutoringSessionOnGoing } from './models/tutoring/tutoring-sessions/tutoring-session-on-going';
import { AuthService } from './services/auth/auth.service';
import { HubsService } from './services/hubs.service';
import { TutoringSessionsService } from './services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [
    MessageService
  ]
})
export class AppComponent implements OnInit {
  public isTutoringEvaluationVisible = false;
  public evaluationHeader = '';
  private sessionId: number;

  public evaluation: number;
  public evaluationComment: string;

  private onGoingSession: TutoringSessionOnGoing;

  private messageCleared = false;

  constructor(
    private authService: AuthService,
    private hubsService: HubsService,
    private tutoringSessionsService: TutoringSessionsService,
    private messageService: MessageService,
    public router: Router
  ) { }

  ngOnInit(): void {
    this.preserveAuthenticationState();

    this.hubsService.startConnections();

    this.initializeTutoringSessionListener();
    this.initializeOnGoingTutoringSession();
    this.initializeLogoutAction();
    this.initializeLoginAction();
    this.initializeRouteChangeSubscription();
    this.initializeOnGoingSessionSubscription();
  }

  private initializeOnGoingSessionSubscription(): void {
    this.tutoringSessionsService.onGoingTutoringSession$.pipe(
      filter(s => !!s),
      filter(_ => !this.onGoingSession),
      tap(s => this.onGoingSession = s),
      tap(_ => this.handleUrlChangedEvent(this.router.url))
    )
      .subscribe();
  }

  private initializeRouteChangeSubscription(): void {
    console.log(this.onGoingSession);
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(event => (event as NavigationEnd).url),
      tap(url => this.handleUrlChangedEvent(url))
    )
      .subscribe();
  }

  private handleUrlChangedEvent(url: string): void {
    if (this.onGoingSession) {
      if (url === this.getTutoringSessionLink()) {
        this.messageService.clear();
        this.messageCleared = true;
      } else if (this.messageCleared) {
        this.messageService.add({ severity: 'info', detail: this.getTutoringSessionLink(), sticky: true, closable: false });
        this.messageCleared = false;
      }
    }
  }

  private preserveAuthenticationState(): void {
    const isCurrentlyAuthenticated = this.authService.isCurrentlyAuthenticated();

    if (isCurrentlyAuthenticated) {
      this.authService.changeAuthState(true);
    }
  }

  private initializeLogoutAction(): void {
    this.authService.isAuthenticated$.pipe(
      filter(isA => !isA),
      tap(_ => this.messageService.clear()),
      tap(_ => this.onGoingSession = null)
    )
      .subscribe();
  }

  private initializeLoginAction(): void {
    this.authService.isAuthenticated$.pipe(
      filter(isA => isA),
      tap(_ => this.initializeOnGoingTutoringSession())
    )
      .subscribe();
  }

  private initializeTutoringSessionListener(): void {
    this.tutoringSessionsService.tutoringSessionFinished().pipe(
      filter(notification => !!notification),
      filter(notification => notification.participantId === this.onGoingSession.participantId),
      tap(_ => this.onGoingSession = null),
      tap(_ => this.messageService.clear()),
      filter(n => n.openNotificationDialog),
      tap(_ => this.isTutoringEvaluationVisible = true),
      tap(notification => this.evaluationHeader = `How do you rate tutoring session with ${notification.tutorName}?`),
      tap(notification => this.sessionId = notification.sessionId)
    )
      .subscribe();
  }

  public evaluateTutor(): void {
    const evaluation: TutoringSessionEvaluation = {
      evaluation: this.evaluation,
      comment: this.evaluationComment
    };

    this.tutoringSessionsService.evaluateTutoringSession(this.sessionId, evaluation).subscribe(_ => {
      this.isTutoringEvaluationVisible = false;
    });
  }

  private initializeOnGoingTutoringSession(): void {
    this.tutoringSessionsService.getOnGoingSession().pipe(
      filter(s => !!s),
      filter(_ => !this.onGoingSession),
      tap(s => this.onGoingSession = s),
      tap(_ => this.handleUrlChangedEvent(this.router.url))
    )
      .subscribe();
  }

  public getTutoringSessionLink(): string {
    if (!this.onGoingSession) {
      return '';
    }

    const subRoute = this.onGoingSession.isStudent
      ? `tutors/${this.onGoingSession.participantId}`
      : `students/${this.onGoingSession.participantId}`;

    return `/modules/${this.onGoingSession.moduleId}/${subRoute}`;
  }
}
