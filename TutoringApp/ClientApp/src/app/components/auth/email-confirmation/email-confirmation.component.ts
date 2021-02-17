import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'primeng/api';
import { map, switchMap } from 'rxjs/operators';
import { EmailConfirmation } from 'src/app/models/auth/email-confirmation';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {
  public messages: Message[];

  constructor(
    private authService: AuthService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.confirmEmail();
  }

  private confirmEmail(): void {
    this.activatedRoute.queryParams.pipe(
      map(params => ({ email: params.email, encodedToken: params.token } as EmailConfirmation)),
      switchMap(emailConfirmation => this.authService.confirmEmail(emailConfirmation))
    )
      .subscribe(
        _ => this.messages = [{
          severity: 'success',
          summary: 'Congratulations!',
          detail: 'Your email has been confirmed successfully. You may login and start learning!'
        }],
        err => this.messages = [{
          severity: 'error',
          summary: 'Oops!',
          detail: `Your email was unable to be confirmed. Error: ${err.error}`
        }]
      );
  }
}
