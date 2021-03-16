import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { AppConstants } from 'src/app/app.constants';
import { StudentCycleEnum } from 'src/app/models/enums/student-cycle-enum';
import { TutoringApplicationNew } from 'src/app/models/tutoring/tutoring-application-new';
import { Tutor } from 'src/app/models/users/tutor';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModulesService } from 'src/app/services/modules/modules.service';
import { UsersService } from 'src/app/services/users/users.service';
import { mapStudentYearToNumber } from 'src/app/utils/map-student-year-to-number';

@Component({
  selector: 'app-tutor-table',
  templateUrl: './tutor-table.component.html',
  styleUrls: ['./tutor-table.component.scss'],
  providers: [
    MessageService,
    ConfirmationService
  ]
})
export class TutorTableComponent implements OnInit {
  public tutors: Tutor[] = [];
  public columns = [
    { field: 'name', header: 'Name' },
    { field: 'faculty', header: 'Faculty' },
    { field: 'studyBranch', header: 'Branch' },
    { field: 'studentCycleDisplay', header: 'Cycle' },
    { field: 'studentYearDisplay', header: 'Year' },
    { field: 'averageScore', header: 'Average Score' },
    { field: 'tutoringSessionCount', header: 'Tutoring Sessions' }
  ];

  private moduleId: number;

  public canApplyForTutoring = false;
  public canResignFromTutoring = false;

  public tutoringApplicationHeader = 'Provide a motivational letter in order to apply for tutoring.';
  public isApplicationDialogVisible = false;

  private lastFilterCheckboxValue = false;

  public isStudent = false;

  @ViewChild(Table)
  private tableComponent: Table;

  constructor(
    private usersService: UsersService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private modulesService: ModulesService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeModuleId();
    this.initializeTutors();
    this.initializeMetadata();
  }

  //#region Initialization
  private initializeModuleId(): void {
    this.activatedRoute.params.subscribe(params => {
      this.moduleId = +params.id;
    });
  }
  private initializeTutors(): void {
    this.usersService.getTutors(this.moduleId).subscribe(tutors => {
      this.tutors = tutors;
      this.tutors.map(tutor => {
        tutor.studentCycleDisplay = StudentCycleEnum[tutor.studentCycle];
        tutor.studentYearDisplay = mapStudentYearToNumber(tutor.studentYear).toString();
      });
    });
  }

  private initializeMetadata(): void {
    this.isStudent = this.authService.currentUserBelongsToRole(AppConstants.StudentRole);
    if (this.isStudent) {
      this.modulesService.getModuleMetadata(this.moduleId).subscribe(metadata => {
        this.canApplyForTutoring = metadata.canApplyForTutoring;
        this.canResignFromTutoring = metadata.canResignFromTutoring;
      });
    }
  }
  //#endregion

  //#region Tutoring management
  public openApplicationDialog(): void {
    this.setApplicationDialogVisibility(true);
  }

  public apply(tutoringApplicationNew: TutoringApplicationNew): void {
    this.closeApplicationDialog();
    this.modulesService.applyForTutoring(this.moduleId, tutoringApplicationNew).subscribe(
      _ => this.handleApplicationSuccess(),
      err => this.messageService.add({ severity: 'error', summary: 'Could not apply for tutoring', detail: err.error })
    );
  }

  private handleApplicationSuccess(): void {
    this.canApplyForTutoring = false;
    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'You have applied for tutoring successfully. Wait for our email!' });
  }

  public closeApplicationDialog(): void {
    this.setApplicationDialogVisibility(false);
  }

  public setApplicationDialogVisibility(isVisible: boolean) {
    this.isApplicationDialogVisible = isVisible;
  }

  public confirmResignation(): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: 'Are you sure you want to resign from tutoring?',
      accept: () => this.resignFromTutoring()
    });
  }

  private resignFromTutoring(): void {
    this.modulesService.resignFromTutoring(this.moduleId).subscribe(
      _ => this.handleResignationSuccess(),
      err => this.messageService.add({ severity: 'error', summary: 'Could not resign from tutoring', detail: err.error })
    );
  }

  private handleResignationSuccess(): void {
    this.canResignFromTutoring = false;
    this.canApplyForTutoring = true;
    this.messageService.add({ severity: 'success', summary: 'Success!', detail: 'You have resigned from tutoring successfully. Sorry to see you go...' });

    const userId = this.authService.getCurrentUserId();
    this.tutors = this.tutors.filter(t => t.id !== userId);
  }
  //#endregion

  //#region Student Tutor management
  public addStudentTutor(tutor: Tutor): void {
    this.modulesService.addStudentTutor(this.moduleId, tutor.id).subscribe({
      next: _ => this.handleAddStudentTutorSuccess(tutor),
      error: err => this.messageService.add({ severity: 'error', summary: 'Could not add tutor', detail: err.error })
    });
  }

  private handleAddStudentTutorSuccess(tutor: Tutor): void {
    tutor.isAddable = false;
    this.messageService.add({
      severity: 'success',
      summary: 'Success!',
      detail: `You have successfully added ${tutor.name} as your tutor!`
    });
  }

  public confirmTutorRemove(tutor: Tutor): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Are you sure you want to remove ${tutor.name} from your tutor list?`,
      accept: () => this.doTutorRemove(tutor)
    });
  }

  private doTutorRemove(tutor: Tutor): void {
    this.modulesService.removeStudentTutor(this.moduleId, tutor.id).subscribe({
      next: _ => this.handleTutorRemoveSuccess(tutor),
      error: err => this.messageService.add({ severity: 'error', summary: 'Could not remove tutor', detail: err.error })
    });
  }

  private handleTutorRemoveSuccess(tutor: Tutor): void {
    tutor.isAddable = true;
    this.filterMyTutors();
    this.messageService.add({
      severity: 'success',
      summary: 'Success!',
      detail: `You have successfully removed ${tutor.name} from your tutor list!`
    });
  }
  //#endregion

  //#region Filtering
  public filterMyTutors(filter = this.lastFilterCheckboxValue): void {
    this.lastFilterCheckboxValue = filter;

    if (filter) {
      this.tableComponent.filter(false, 'isAddable', 'equals');
    } else {
      this.tableComponent.reset();
    }
  }
  //#endregion

  //#region Event handlers
  public navigateToTutorView(tutorId: string): void {
    this.router.navigateByUrl(`tutors/${tutorId}`);
  }
  //#endregion
}
