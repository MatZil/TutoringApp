import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';
import { AppConstants } from 'src/app/app.constants';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-tutor-view',
  templateUrl: './tutor-view.component.html',
  styleUrls: ['./tutor-view.component.scss']
})
export class TutorViewComponent implements OnInit {
  public isStudent = false;

  public tutorId: number;

  constructor(
    private authService: AuthService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.initializeTutorId();
    this.initializeRole();
  }

  private initializeRole(): void {
    this.isStudent = this.authService.currentUserBelongsToRole(AppConstants.StudentRole);
  }

  private initializeTutorId(): void {
    this.activatedRoute.params.pipe(
      tap(params => this.tutorId = params.id)
    )
      .subscribe();
  }
}
