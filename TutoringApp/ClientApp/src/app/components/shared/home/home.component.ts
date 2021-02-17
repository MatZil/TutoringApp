import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'primeng/api';
import { HomepageMessageUseCaseEnum } from 'src/app/models/enums/homepage-message-use-case.enum';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public messages: Message[];

  constructor(
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.initializeMessage();
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
