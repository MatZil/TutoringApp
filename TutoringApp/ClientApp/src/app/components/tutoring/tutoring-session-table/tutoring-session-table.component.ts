import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { TutoringSessionEvaluationEnum } from 'src/app/models/enums/tutoring-session-evaluation-enum';
import { TutoringSessionStatusEnum } from 'src/app/models/enums/tutoring-session-status-enum';
import { TutoringSession } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-tutoring-session-table',
  templateUrl: './tutoring-session-table.component.html',
  styleUrls: ['./tutoring-session-table.component.scss'],
  providers: [
    ConfirmationService
  ]
})
export class TutoringSessionTableComponent implements OnInit, OnChanges {
  @Input() public tutoringSessions: TutoringSession[] = [];
  @Input() public title: string;
  @Input() public isTutoringTable = false;

  @Output()
  private sessionCancelled = new EventEmitter<boolean>();

  public columns = [
    { field: 'moduleName', header: 'Module' },
    { field: 'participantName', header: 'Participant' },
    { field: 'creationDate', header: 'Created' },
    { field: 'isSubscribed', header: 'Is Subscribed' },
    { field: 'sessionDate', header: 'Session Date' },
    { field: 'statusDisplay', header: 'Status' },
    { field: 'statusChangeDate', header: 'Status Changed' },
    { field: 'evaluationDisplay', header: 'Evaluation' }
  ];

  constructor(
    private tutoringSessionsService: TutoringSessionsService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    if (this.tutoringSessions.length) {
      this.initializeTutoringSessions();
    }
  }

  private initializeTutoringSessions(): void {
    this.tutoringSessions.forEach(session => {
      session.isActive = session.status === TutoringSessionStatusEnum.Upcoming;
      session.statusDisplay = TutoringSessionStatusEnum[session.status];
      session.evaluationDisplay = session.evaluation
        ? TutoringSessionEvaluationEnum[session.evaluation]
        : '-';
    });
  }

  public invertTutoringSessions(sessionId: number): void {
    this.tutoringSessionsService.invertTutoringSession(sessionId).subscribe();
  }

  public confirmSessionCancelling(session: TutoringSession): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: 'Are you sure you want to cancel this tutoring session?',
      accept: () => this.cancelSession(session)
    });
  }

  private cancelSession(session: TutoringSession): void {
    this.tutoringSessionsService.cancelTutoringSession(session.id).subscribe(_ => {
      this.sessionCancelled.emit();
    });
  }
}
