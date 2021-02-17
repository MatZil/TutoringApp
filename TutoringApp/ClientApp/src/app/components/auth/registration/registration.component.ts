import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { UserRegistration } from 'src/app/models/auth/user-registration';
import { HomepageMessageUseCaseEnum } from 'src/app/models/enums/homepage-message-use-case.enum';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
  providers: [MessageService]
})
export class RegistrationComponent implements OnInit {
  public registrationFormGroup: FormGroup;

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
    this.registrationFormGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: [],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required]
    });
  }

  public register() {
    const isValid = this.validateRegistration();

    if (isValid) {
      this.authService.register(this.registrationFormGroup.value).subscribe(
        _ => this.router.navigate([''], { queryParams: { useCase: HomepageMessageUseCaseEnum.RegistrationSuccess } }),
        err => this.messageService.add({ severity: 'error', summary: 'Error', detail: `${err.error}` })
        );
    }
  }

  private validateRegistration(): boolean {
    if (!this.registrationFormGroup.valid) {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Please enter the required information' });
      return false;
    } else if (this.registrationFormGroup.controls.password.value !== this.registrationFormGroup.controls.confirmPassword.value) {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Passwords do not match' });
      return false;
    }

    return true;
  }
}
