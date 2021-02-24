import { Component, OnInit } from '@angular/core';
import { NamedEntity } from 'src/app/models/shared/named-entity';
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

  constructor(
    private modulesService: ModulesService
  ) { }

  ngOnInit(): void {
    this.initializeModules();
  }

  //#region Initialization
  private initializeModules(): void {
    this.modulesService.getModules().subscribe(modules => {
      this.allModules = modules;
      this.virtualModules = modules;
    });
  }
  //#endregion

  //#region Event handlers
  public filterModules(): void {
    this.virtualModules = this.allModules.filter(m => m.name.toLowerCase().includes(this.filterString.toLowerCase()));
  }
  //#endregion
}
