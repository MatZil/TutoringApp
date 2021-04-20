import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AppConstants } from 'src/app/app.constants';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModulesService } from 'src/app/services/modules/modules.service';
import { TutorTableComponent } from '../../users/tutor-table/tutor-table.component';

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

  @ViewChild(TutorTableComponent)
  public tutorTableComponent: TutorTableComponent;

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
}
