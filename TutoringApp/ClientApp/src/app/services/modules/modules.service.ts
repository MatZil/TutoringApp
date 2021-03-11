import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModuleNew } from 'src/app/models/modules/module-new';
import { NamedEntity } from 'src/app/models/shared/named-entity';
import { TutoringApplicationNew } from 'src/app/models/tutoring/tutoring-application-new';
import { UserModuleMetadata } from 'src/app/models/users/user-module-metadata';
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

  public getModuleMetadata(id: number): Observable<UserModuleMetadata> {
    return this.httpService.get(this.modulesController, `${id}/metadata`);
  }

  public applyForTutoring(id: number, tutoringApplicationNew: TutoringApplicationNew): Observable<any> {
    return this.httpService.post(this.modulesController, `${id}/apply`, tutoringApplicationNew);
  }

  public resignFromTutoring(id: number): Observable<any> {
    return this.httpService.post(this.modulesController, `${id}/resign`, null);
  }

  public addStudentTutor(moduleId: number, tutorId: string): Observable<any> {
    return this.httpService.post(this.modulesController, `${moduleId}/tutors/${tutorId}`, null);
  }

  public removeStudentTutor(moduleId: number, tutorId: string): Observable<any> {
    return this.httpService.delete(this.modulesController, `${moduleId}/tutors/${tutorId}`, null);
  }

  public removeTutorStudent(moduleId: number, studentId: string): Observable<any> {
    return this.httpService.delete(this.modulesController, `${moduleId}/students/${studentId}`, null);
  }
}
