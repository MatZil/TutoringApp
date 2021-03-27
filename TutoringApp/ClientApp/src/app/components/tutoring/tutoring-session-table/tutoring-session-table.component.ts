import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { TutoringSessionEvaluationEnum } from 'src/app/models/enums/tutoring-session-evaluation-enum';
import { TutoringSessionStatusEnum } from 'src/app/models/enums/tutoring-session-status-enum';
import { TutoringSession } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-tutoring-session-table',
  templateUrl: './tutoring-session-table.component.html',
  styleUrls: ['./tutoring-session-table.component.scss']
})
export class TutoringSessionTableComponent implements OnInit, OnChanges {
  @Input() public tutoringSessions: TutoringSession[] = [];
  @Input() public title: string;

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
    private tutoringSessionsService: TutoringSessionsService
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
}
