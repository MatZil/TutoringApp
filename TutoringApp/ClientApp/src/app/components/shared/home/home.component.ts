import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'primeng/api';
import { Observable } from 'rxjs';
import { HomepageMessageUseCaseEnum } from 'src/app/models/enums/homepage-message-use-case.enum';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public messages: Message[];
  public isAuthenticated$: Observable<boolean>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initializeMessage();

    this.isAuthenticated$ = this.authService.isAuthenticated$;
  }

  private initializeMessage(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      if (params.useCase === HomepageMessageUseCaseEnum.RegistrationSuccess.toString()) {
        this.messages = [{
          severity: 'success',
          summary: 'Success!',
          detail: 'You have successfully registered! Please confirm your email.'
        }];
      }
    });
  }
}
