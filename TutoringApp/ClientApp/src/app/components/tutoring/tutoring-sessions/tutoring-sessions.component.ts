import { Component, OnInit } from '@angular/core';
import { TutoringSession } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-tutoring-sessions',
  templateUrl: './tutoring-sessions.component.html',
  styleUrls: ['./tutoring-sessions.component.scss']
})
export class TutoringSessionsComponent implements OnInit {
  public tutoringSessions: TutoringSession[] = [];

  constructor(
    private tutoringSessionsService: TutoringSessionsService
  ) { }

  ngOnInit(): void {
    this.initializeTutoringSessions();
  }

  public initializeTutoringSessions(): void {
    this.tutoringSessionsService.getTutoringSessions().subscribe(sessions => {
      this.tutoringSessions = sessions;
    });
  }
}
