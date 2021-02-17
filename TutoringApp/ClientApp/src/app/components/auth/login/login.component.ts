import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeFormGroup();
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
        _ => this.router.navigate(['']),
        err => this.messageService.add({ severity: 'error', summary: 'Error', detail: `${err.error}` })
        );
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
