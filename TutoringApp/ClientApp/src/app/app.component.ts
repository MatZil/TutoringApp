import { Component, OnInit } from '@angular/core';
import { filter, tap } from 'rxjs/operators';
import { TutoringSessionEvaluation } from './models/tutoring/tutoring-sessions/tutoring-session-evaluation';
import { AuthService } from './services/auth/auth.service';
import { HubsService } from './services/hubs.service';
import { TutoringSessionsService } from './services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public isTutoringEvaluationVisible = false;
  public evaluationHeader = '';
  private sessionId: number;

  public evaluation: number;
  public evaluationComment: string;

  constructor(
    private authService: AuthService,
    private hubsService: HubsService,
    private tutoringSessionsService: TutoringSessionsService
  ) { }

  ngOnInit(): void {
    this.preserveAuthenticationState();

    this.hubsService.startConnections();

    this.initializeTutoringSessionListener();
  }

  private preserveAuthenticationState(): void {
    const isCurrentlyAuthenticated = this.authService.isCurrentlyAuthenticated();

    if (isCurrentlyAuthenticated) {
      this.authService.changeAuthState(true);
    }
  }
  
  private initializeTutoringSessionListener(): void {
    this.tutoringSessionsService.tutoringSessionFinished().pipe(
      filter(notification => !!notification),
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
}
