import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { tap } from 'rxjs/operators';
import { TutoringSessionNew } from 'src/app/models/tutoring/tutoring-sessions/tutoring-session-new';
import { ModulesService } from 'src/app/services/modules/modules.service';
import { TutoringSessionsService } from 'src/app/services/tutoring/tutoring-sessions.service';

@Component({
  selector: 'app-student-view',
  templateUrl: './student-view.component.html',
  styleUrls: ['./student-view.component.scss'],
  providers: [
    MessageService
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

  constructor(
    private activatedRoute: ActivatedRoute,
    private tutoringSessionsService: TutoringSessionsService,
    private messageService: MessageService,
    private modulesService: ModulesService
  ) { }

  ngOnInit(): void {
    this.initializeRouteParams();
  }

  private initializeRouteParams(): void {
    this.activatedRoute.params.pipe(
      tap(params => this.studentId = params.id),
      tap(params => this.moduleId = +params.moduleId)
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

    this.modulesService.updateAssignments(this.moduleId, this.studentId, formData).subscribe(_ => {
      this.isAssignmentUploadDialogVisible = false;
    });
  }
}
