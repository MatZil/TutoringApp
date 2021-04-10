import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { tap } from 'rxjs/operators';
import { Assignment } from 'src/app/models/tutoring/assignments/assignment';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModulesService } from 'src/app/services/modules/modules.service';

@Component({
  selector: 'app-tutor-view',
  templateUrl: './tutor-view.component.html',
  styleUrls: ['./tutor-view.component.scss'],
  providers: [
    MessageService
  ]
})
export class TutorViewComponent implements OnInit {
  public tutorId: string;
  public moduleId: number;

  public assignments: Assignment[] = [];

  public isSubmissionUploadDialogVisible = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private modulesService: ModulesService,
    private authService: AuthService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.initializeRouteParams();
    this.initializeAssignments();
  }

  private initializeRouteParams(): void {
    this.activatedRoute.params.pipe(
      tap(params => this.tutorId = params.id),
      tap(params => this.moduleId = +params.moduleId)
    )
      .subscribe();
  }

  private initializeAssignments(): void {
    this.modulesService.getAssignments(this.moduleId, this.tutorId, this.authService.getCurrentUserId()).pipe(
      tap(assignments => this.assignments = assignments)
    )
      .subscribe();
  }

  public uploadSubmission(event: any, assignmentId: number): void {
    const formData = new FormData();
    const file = event.files.pop();

    formData.append('file', file, file.name);

    this.modulesService.uploadSubmission(assignmentId, formData).subscribe(
      _ => {
        this.isSubmissionUploadDialogVisible = false;
        this.initializeAssignments();
      },
      err => this.messageService.add({ severity: 'error', summary: 'Error', detail: err.error })
    );
  }
}
