import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [MessageService]
})
export class LoginComponent implements OnInit {
  public loginFormGroup: FormGroup;
  private returnUrl: string;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.initializeFormGroup();
    this.initializeReturnUrl();
  }

  private initializeReturnUrl(): void {
    this.activatedRoute.queryParams.subscribe(qp => {
      this.returnUrl = qp.returnUrl;
    });
  }

  private initializeFormGroup(): void {
    this.loginFormGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  public login() {
    const isValid = this.validateRegistration();

    if (isValid) {
      this.authService.login(this.loginFormGroup.value).subscribe(
        _ => this.handleSuccess(),
        err => this.messageService.add({ severity: 'error', summary: 'Error', detail: `${err.error}` })
        );
    }
  }

  private handleSuccess(): void {
    if (this.returnUrl) {
      this.router.navigateByUrl(this.returnUrl);
    } else {
      this.router.navigateByUrl('');
    }
  }

  private validateRegistration(): boolean {
    if (!this.loginFormGroup.valid) {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Please enter the required information' });
      return false;
    }

    return true;
  }
}
