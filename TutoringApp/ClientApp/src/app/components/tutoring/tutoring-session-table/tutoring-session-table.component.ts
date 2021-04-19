import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { MessageService } from 'primeng/api';
import { TutoringSessionEvaluationEnum } from 'src/app/models/enums/tutoring-session-evaluation-enum';
import { TutoringSessionStatusEnum } from 'src/app/models/enums/tutoring-session-status-enum';
import { TutoringSession } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session';
import { TutoringSessionCancel } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-cancel';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-tutoring-session-table',
  templateUrl: './tutoring-session-table.component.html',
  styleUrls: ['./tutoring-session-table.component.scss'],
  providers: [
    MessageService
  ]
})
export class TutoringSessionTableComponent implements OnInit, OnChanges {
  @Input() public tutoringSessions: TutoringSession[] = [];
  @Input() public title: string;
  @Input() public isTutoringTable = false;

  public isCancelDialogVisible = false;
  public cancellationReason = '';
  private sessionToCancelId: number;

  @Output()
  private sessionCancelled = new EventEmitter<boolean>();

  public columns = [
    { field: 'moduleName', header: 'Module' },
    { field: 'participantName', header: 'Participant' },
    { field: 'creationDate', header: 'Created' },
    { field: 'isSubscribed', header: 'Is Subscribed' },
    { field: 'sessionDate', header: 'Session Date' },
    { field: 'statusDisplay', header: 'Status' },
    { field: 'cancellationReason', header: 'Cancellation reason' },
    { field: 'statusChangeDate', header: 'Status Changed' },
    { field: 'evaluationDisplay', header: 'Evaluation' }
  ];

  constructor(
    private tutoringSessionsService: TutoringSessionsService,
    private messageService: MessageService
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

  public invertTutoringSession(sessionId: number): void {
    this.tutoringSessionsService.invertTutoringSession(sessionId).subscribe();
  }

  public openCancellationDialog(session: TutoringSession): void {
    this.isCancelDialogVisible = true;
    this.sessionToCancelId = session.id;
  }

  public cancelSession(): void {
    const tutoringSessionCancel: TutoringSessionCancel = {
      reason: this.cancellationReason
    };

    this.tutoringSessionsService.cancelTutoringSession(this.sessionToCancelId, tutoringSessionCancel).subscribe(
      _ => {
        this.isCancelDialogVisible = false;
        this.sessionToCancelId = null;
        this.cancellationReason = '';
        this.sessionCancelled.emit();
      },
      err => this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error })
    );
  }
}
