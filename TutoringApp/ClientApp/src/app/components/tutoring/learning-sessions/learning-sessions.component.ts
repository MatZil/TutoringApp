import { Component, OnInit } from '@angular/core';
import { TutoringSession } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-learning-sessions',
  templateUrl: './learning-sessions.component.html',
  styleUrls: ['./learning-sessions.component.scss']
})
export class LearningSessionsComponent implements OnInit {
  public sessions: TutoringSession[] = [];

  constructor(
    private tutoringSessionsService: TutoringSessionsService
  ) { }

  ngOnInit(): void {
    this.initializeTutoringSessions();
  }

  public initializeTutoringSessions(): void {
    this.tutoringSessionsService.getLearningSessions().subscribe(sessions => {
      this.sessions = sessions;
    });
  }
}
