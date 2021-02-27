import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService, SelectItem } from 'primeng/api';
import { HomepageMessageUseCaseEnum } from 'src/app/models/enums/homepage-message-use-case.enum';
import { StudentCycleEnum } from 'src/app/models/enums/student-cycle-enum';
import { StudentYearEnum } from 'src/app/models/enums/student-year-enum';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
  providers: [MessageService]
})
export class RegistrationComponent implements OnInit {
  public registrationFormGroup: FormGroup;

  public cycleOptions: SelectItem[] = [
    { label: StudentCycleEnum[StudentCycleEnum.Bachelor], value: StudentCycleEnum.Bachelor },
    { label: StudentCycleEnum[StudentCycleEnum.Master], value: StudentCycleEnum.Master },
    { label: StudentCycleEnum[StudentCycleEnum.Doctorate], value: StudentCycleEnum.Doctorate }
  ];

  public yearOptions: SelectItem[] = [
    { label: '1', value: StudentYearEnum.FirstYear },
    { label: '2', value: StudentYearEnum.SecondYear },
    { label: '3', value: StudentYearEnum.ThirdYear },
    { label: '4', value: StudentYearEnum.FourthYear }
  ];

  public facultyOptions: SelectItem[] = [
    { label: 'Informatics', value: 'Informatics' }
  ];

  public studyBranchOptions: SelectItem[] = [
    { label: 'Software Systems', value: 'Software Systems'},
    { label: 'IT Systems', value: 'IT Systems'},
    { label: 'Informatics', value: 'Informatics'},
    { label: 'Information Systems', value: 'Information Systems'}
  ];

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
      lastName: ['', Validators.required],
      studentCycle: ['', Validators.required],
      studentYear: ['', Validators.required],
      faculty: ['', Validators.required],
      studyBranch: ['', Validators.required]
    });
  }

  public register() {
    const isValid = this.validateRegistration();

    if (isValid) {
      this.authService.register(this.registrationFormGroup.value).subscribe(
        _ => this.router.navigate([''], { queryParams: { useCase: HomepageMessageUseCaseEnum.RegistrationSuccess } }),
        err => this.messageService.add({ severity: 'error', summary: 'Errorrraaa', detail: `${err.error}` })
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
