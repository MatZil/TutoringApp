import { Component, OnInit } from '@angular/core';
import { AppConstants } from 'src/app/app.constants';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-tutor-view',
  templateUrl: './tutor-view.component.html',
  styleUrls: ['./tutor-view.component.scss']
})
export class TutorViewComponent implements OnInit {
  public isStudent = false;

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initializeRole();
  }

  private initializeRole(): void {
    this.isStudent = this.authService.currentUserBelongsToRole(AppConstants.StudentRole);
  }
}
