import { Component, OnInit } from '@angular/core';
import { AppConstants } from 'src/app/app.constants';
import { NamedEntity } from 'src/app/models/shared/named-entity';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModulesService } from 'src/app/services/modules/modules.service';

@Component({
  selector: 'app-module-list',
  templateUrl: './module-list.component.html',
  styleUrls: ['./module-list.component.scss']
})
export class ModuleListComponent implements OnInit {
  private allModules: NamedEntity[] = [];
  public virtualModules: NamedEntity[] = [];

  public filterString = '';

  public isAdmin = false;

  constructor(
    private modulesService: ModulesService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initializeModules();
    this.initializeIsAdmin();
  }

  //#region Initialization
  private initializeModules(): void {
    this.modulesService.getModules().subscribe(modules => {
      this.allModules = modules;
      this.virtualModules = modules;
    });
  }

  private initializeIsAdmin(): void {
    this.isAdmin = this.authService.currentUserBelongsToRole(AppConstants.AdminRole);
  }
  //#endregion

  //#region Event handlers
  public filterModules(): void {
    this.virtualModules = this.allModules.filter(m => m.name.toLowerCase().includes(this.filterString.toLowerCase()));
  }
  //#endregion
}
