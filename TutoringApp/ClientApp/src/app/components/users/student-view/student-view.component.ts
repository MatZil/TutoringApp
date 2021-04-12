import { LocationStrategy } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { tap } from 'rxjs/operators';
import { Assignment } from 'src/app/models/tutoring/assignments/assignment';
import { TutoringSessionNew } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-new';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModulesService } from 'src/app/services/modules/modules.service';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';
import { UsersService } from 'src/app/services/users/users.service';

@Component({
  selector: 'app-student-view',
  templateUrl: './student-view.component.html',
  styleUrls: ['./student-view.component.scss'],
  providers: [
    MessageService,
    ConfirmationService
  ]
})
export class StudentViewComponent implements OnInit {
  public studentId: string;
  public moduleId: number;

  public isSessionCreationDialogVisible = false;
  public isAssignmentUploadDialogVisible = false;
  public minSessionDate = new Date();
  public tutoringSessionNew: TutoringSessionNew = {
    studentId: undefined,
    moduleId: undefined,
    isSubscribed: undefined,
    sessionDate: undefined
  };

  public evaluation: number;
  public isEvaluationDialogVisible = false;
  public assignmentToEvaluateId: number;

  public assignments: Assignment[] = [];

  public currentRoute: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private tutoringSessionsService: TutoringSessionsService,
    private messageService: MessageService,
    private modulesService: ModulesService,
    private authService: AuthService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private usersService: UsersService,
    private locationStrategy: LocationStrategy
  ) { }

  ngOnInit(): void {
    this.initializeRouteParams();
    this.initializeAssignments();

    this.currentRoute = this.router.url;
  }

  private initializeRouteParams(): void {
    this.activatedRoute.params.pipe(
      tap(params => this.studentId = params.id),
      tap(params => this.moduleId = +params.moduleId)
    )
      .subscribe();
  }

  private initializeAssignments(): void {
    this.modulesService.getAssignments(this.moduleId, this.authService.getCurrentUserId(), this.studentId).pipe(
      tap(assignments => this.assignments = assignments)
    )
      .subscribe();
  }

  public initializeTutoringSessionCreation(): void {
    const today = new Date();

    this.minSessionDate.setHours(today.getHours() + 2);
    this.tutoringSessionNew = {
      isSubscribed: false,
      moduleId: this.moduleId,
      studentId: this.studentId,
      sessionDate: this.minSessionDate
    };

    this.isSessionCreationDialogVisible = true;
  }

  public createNewTutoringSession(): void {
    this.tutoringSessionsService.createTutoringSession(this.tutoringSessionNew).subscribe(_ => {
      this.isSessionCreationDialogVisible = false;

      this.messageService.add({ severity: 'success', summary: 'Success!', detail: 'You have successfully registered a tutoring session.' });
    });
  }

  public updateAssignments(event: any): void {
    const formData = new FormData();

    const originalLength = event.files.length;
    for (let i = 0; i < originalLength; i++) {
      const file = event.files.pop();

      formData.append('file', file, file.name);
    }

    this.modulesService.uploadAssignments(this.moduleId, this.studentId, formData).subscribe(
      _ => {
        this.isAssignmentUploadDialogVisible = false;
        this.initializeAssignments();
      },
      err => this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error })
    );
  }

  public confirmAssignmentDelete(id: number): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: 'Are you sure you want to delete this assignment?',
      accept: () => this.deleteAssignment(id)
    });
  }

  private deleteAssignment(id: number): void {
    this.modulesService.deleteAssignment(id).pipe(
      tap(_ => this.assignments = this.assignments.filter(a => a.id !== id))
    )
      .subscribe();
  }

  public openEvaluationDialog(assignmentId: number): void {
    this.assignmentToEvaluateId = assignmentId;
    this.evaluation = null;
    this.isEvaluationDialogVisible = true;
  }

  public evaluateSubmission(): void {
    this.modulesService.evaluateSubmission(this.assignmentToEvaluateId, Math.floor(this.evaluation)).pipe(
      tap(_ => this.initializeAssignments()),
      tap(_ => this.isEvaluationDialogVisible = false)
    )
      .subscribe();
  }

  public downloadFile(assignmentId: number, fileName: string): void {
    this.modulesService.downloadAssignmentFile(assignmentId, fileName).pipe(

    )
      .subscribe();
  }

  public confirmStudentIgnore(): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: 'Do you really want to ignore this student? This applies to all modules.',
      accept: () => this.ignoreStudent()
    });
  }

  private ignoreStudent(): void {
    this.usersService.ignoreStudent(this.studentId).pipe(
      tap(_ => this.locationStrategy.back())
    )
      .subscribe();
  }
}
