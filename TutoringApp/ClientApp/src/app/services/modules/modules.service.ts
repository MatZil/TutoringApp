import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModuleNew } from 'src/app/models/modules/module-new';
import { NamedEntity } from 'src/app/models/shared/named-entity';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class ModulesService {
  private modulesController = 'Modules';

  constructor(
    private httpService: HttpService
  ) { }

  public getModules(): Observable<NamedEntity[]> {
    return this.httpService.get(this.modulesController, '');
  }

  public createModule(moduleNew: ModuleNew): Observable<number> {
    return this.httpService.post(this.modulesController, '', moduleNew);
  }

  public deleteModule(id: number): Observable<any> {
    return this.httpService.delete(this.modulesController, id.toString());
  }
}
