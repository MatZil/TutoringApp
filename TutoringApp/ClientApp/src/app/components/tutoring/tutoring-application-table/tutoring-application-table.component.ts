import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { StudentCycleEnum } from 'src/app/models/enums/student-cycle-enum';
import { TutoringApplication } from 'src/app/models/tutoring/tutoring-application';
import { TutoringApplicationsService } from 'src/app/services/tutoring/tutoring-applications.service';
import { mapStudentYearToNumber } from 'src/app/utils/map-student-year-to-number';

@Component({
  selector: 'app-tutoring-application-table',
  templateUrl: './tutoring-application-table.component.html',
  styleUrls: ['./tutoring-application-table.component.scss'],
  providers: [
    ConfirmationService,
    MessageService
  ]
})
export class TutoringApplicationTableComponent implements OnInit {
  public tutoringApplications: TutoringApplication[] = [];
  public columns = [
    { field: 'moduleName', header: 'Module' },
    { field: 'studentName', header: 'Student' },
    { field: 'email', header: 'Email' },
    { field: 'studentCycleDisplay', header: 'Cycle' },
    { field: 'studentYearDisplay', header: 'Year' },
    { field: 'faculty', header: 'Faculty' },
    { field: 'studyBranch', header: 'Study Branch' },
    { field: 'requestDate', header: 'Requested' }
  ];

  public motivationalLetterToOpen: string;
  public isMotivationalLetterDialogVisible = false;

  constructor(
    private tutoringApplicationsService: TutoringApplicationsService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.initializeTutoringApplications();
  }

  //#region Initialization
  private initializeTutoringApplications(): void {
    this.tutoringApplicationsService.getTutoringApplications().subscribe(tutoringApplications => {
      this.tutoringApplications = tutoringApplications;
      this.tutoringApplications.map(tutoringApplication => {
        tutoringApplication.studentCycleDisplay = StudentCycleEnum[tutoringApplication.studentCycle];
        tutoringApplication.studentYearDisplay = mapStudentYearToNumber(tutoringApplication.studentYear).toString();
      });
    });
  }
  //#endregion

  //#region Event handlers
  public openMotivationalLetter(id: number): void {
    this.motivationalLetterToOpen = this.tutoringApplications.find(ta => ta.id === id).motivationalLetter;

    this.isMotivationalLetterDialogVisible = true;
  }

  public confirmTutoringApplication(application: TutoringApplication): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Are you sure you want to promote user ${application.studentName} to ${application.moduleName}'s tutor?`,
      accept: () => this.doConfirmTutoringApplication(application.id)
    });
  }

  private doConfirmTutoringApplication(id: number): void {
    this.tutoringApplicationsService.confirmTutoringApplication(id).subscribe(
      _ => this.handleConfirmationSuccess(id),
      err => this.messageService.add({ severity: 'error', summary: 'Could not promote a user', detail: err.error })
    );
  }

  private handleConfirmationSuccess(id: number): void {
    this.tutoringApplications = this.tutoringApplications.filter(ta => ta.id !== id);

    this.messageService.add({ severity: 'success', summary: 'success', detail: 'You have successfully promoted a user!' });
  }

  public rejectTutoringApplication(application: TutoringApplication): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Are you sure you want to reject user ${application.studentName} to be ${application.moduleName}'s tutor?`,
      accept: () => this.doRejectTutoringApplication(application.id)
    });
  }

  private doRejectTutoringApplication(id: number): void {
    this.tutoringApplicationsService.rejectTutoringApplication(id).subscribe(
      _ => this.handleRejectionSuccess(id),
      err => this.messageService.add({ severity: 'error', summary: 'Could not promote a user', detail: err.error })
    );
  }

  private handleRejectionSuccess(id: number): void {
    this.tutoringApplications = this.tutoringApplications.filter(ta => ta.id !== id);

    this.messageService.add({ severity: 'success', summary: 'success', detail: 'You have successfully rejected a user!' });
  }
  //#endregion
}
