import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AppConstants } from 'src/app/app.constants';
import { TutoringApplicationNew } from 'src/app/models/tutoring/tutoring-application-new';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModulesService } from 'src/app/services/modules/modules.service';

@Component({
  selector: 'app-module-view',
  templateUrl: './module-view.component.html',
  styleUrls: ['./module-view.component.scss'],
  providers: [
    MessageService,
    ConfirmationService
  ]
})
export class ModuleViewComponent implements OnInit {
  private moduleId: number;
  public isAdmin = false;

  public canApplyForTutoring = false;
  public canResignFromTutoring = false;

  public tutoringApplicationHeader = 'Provide a motivational letter in order to apply for tutoring.';
  public isApplicationDialogVisible = false;

  constructor(
    private modulesService: ModulesService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeModuleId();
    this.initializeIsAdmin();
    this.initializeMetadata();
  }

  //#region Initialization
  private initializeModuleId(): void {
    this.activatedRoute.params.subscribe(params => {
      this.moduleId = +params.id;
    });
  }

  private initializeIsAdmin(): void {
    this.isAdmin = this.authService.currentUserBelongsToRole(AppConstants.AdminRole);
  }

  private initializeMetadata(): void {
    const isStudent = this.authService.currentUserBelongsToRole(AppConstants.StudentRole);
    if (isStudent) {
      this.modulesService.getModuleMetadata(this.moduleId).subscribe(metadata => {
        this.canApplyForTutoring = metadata.canApplyForTutoring;
        this.canResignFromTutoring = metadata.canResignFromTutoring;
      });
    }
  }
  //#endregion

  //#region Delete management
  public confirmDelete(): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: 'Are you sure you want to delete this module?',
      accept: () => this.deleteModule()
    });
  }

  private deleteModule(): void {
    this.modulesService.deleteModule(this.moduleId).subscribe(
      _ => this.router.navigateByUrl(''),
      err => this.messageService.add({ severity: 'error', summary: 'Could not delete module', detail: err.error })
    );
  }
  //#endregion

  //#region Tutoring management
  public openApplicationDialog(): void {
    this.setApplicationDialogVisibility(true);
  }

  public apply(tutoringApplicationNew: TutoringApplicationNew): void {
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
  //#endregion
}
