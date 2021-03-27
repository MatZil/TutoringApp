import { not } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Message } from 'primeng/api';
import { Observable } from 'rxjs';
import { filter, tap } from 'rxjs/operators';
import { AppConstants } from 'src/app/app.constants';
import { HomepageMessageUseCaseEnum } from 'src/app/models/enums/homepage-message-use-case.enum';
import { TutoringSessionEvaluation } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-evaluation';
import { AuthService } from 'src/app/services/auth/auth.service';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public messages: Message[] = [];
  public isAuthenticated$: Observable<boolean>;
  public isAdmin = false;
  public isStudent = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initializeMessage();

    this.isAuthenticated$ = this.authService.isAuthenticated$;
    this.isAdmin = this.authService.currentUserBelongsToRole(AppConstants.AdminRole);
    this.isStudent = this.authService.currentUserBelongsToRole(AppConstants.StudentRole);
  }

  private initializeMessage(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.chooseMessage(params);
    });
  }

  private chooseMessage(params: Params): void {
    if (params.useCase === HomepageMessageUseCaseEnum.RegistrationSuccess.toString()) {
      this.messages = [{
        severity: 'success',
        summary: 'Success!',
        detail: 'You have successfully registered! Please confirm your email.'
      }];
    }
  }
}
