import { Component, Output, ViewChild, EventEmitter, Input, OnInit, OnChanges } from '@angular/core';
import { MessageService } from 'primeng/api';
import { OverlayPanel } from 'primeng/overlaypanel';
import { ModuleNew } from 'src/app/models/modules/module-new';
import { NamedEntity } from 'src/app/models/shared/named-entity';
import { ModulesService } from 'src/app/services/modules/modules.service';

@Component({
  selector: 'app-module-create',
  templateUrl: './module-create.component.html',
  styleUrls: ['./module-create.component.scss'],
  providers: [
    MessageService
  ]
})
export class ModuleCreateComponent implements OnChanges {
  public name = '';
  public nameExists = false;

  @Input()
  private existingModules: NamedEntity[];
  private existingNames: string[];

  @ViewChild(OverlayPanel)
  private overlayPanelComponent: OverlayPanel;

  @Output()
  private moduleCreated = new EventEmitter<NamedEntity>();

  constructor(
    private modulesService: ModulesService,
    private messageService: MessageService
  ) { }

  ngOnChanges(): void {
    if (this.existingModules) {
      this.existingNames = this.existingModules.map(m => m.name);
    }
  }

  public toggleOverlay(event: any): void {
    this.overlayPanelComponent.toggle(event);
  }

  public createModule(): void {
    const moduleNew: ModuleNew = {
      name: this.name
    };

    this.modulesService.createModule(moduleNew).subscribe(
      moduleId => this.handleSuccess(moduleId),
      err => this.messageService.add({ severity: 'error', summary: 'Could not create module', detail: err.error })
    );
  }

  private handleSuccess(moduleId: number): void {
    const createdModule: NamedEntity = {
      id: moduleId,
      name: this.name
    };

    this.moduleCreated.emit(createdModule);
    this.name = '';
    this.overlayPanelComponent.hide();
  }

  public validateNameExists(): void {
    this.nameExists = this.existingNames.map(n => n.toLowerCase()).includes(this.name.toLowerCase());
  }
}
