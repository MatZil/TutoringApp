import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ModuleNew } from 'src/app/models/modules/module-new';
import { NamedEntity } from 'src/app/models/shared/named-entity';
import { Assignment } from 'src/app/models/tutoring/assignments/assignment';
import { TutoringApplicationNew } from 'src/app/models/tutoring/tutoring-application-new';
import { UserModuleMetadata } from 'src/app/models/users/user-module-metadata';
import { HttpService } from '../http.service';
import { saveAs } from 'file-saver';
import { Module } from 'src/app/models/modules/module';

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

  public getModule(moduleId: number): Observable<Module> {
    return this.httpService.get(this.modulesController, moduleId.toString());
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

  public uploadAssignments(moduleId: number, studentId: string, fileFormData: FormData): Observable<any> {
    return this.httpService.patch(this.modulesController, `${moduleId}/students/${studentId}/assignments`, fileFormData);
  }

  public getAssignments(moduleId: number, tutorId: string, studentId: string): Observable<Assignment[]> {
    return this.httpService.get(this.modulesController, `${moduleId}/tutors/${tutorId}/students/${studentId}/assignments`);
  }

  public uploadSubmission(assignmentId: number, fileFormData: FormData): Observable<any> {
    return this.httpService.post(this.modulesController, `assignments/${assignmentId}/submit`, fileFormData);
  }

  public evaluateSubmission(assignmentId: number, evaluation: number): Observable<any> {
    return this.httpService.put(this.modulesController, `assignments/${assignmentId}/evaluate?evaluation=${evaluation}`, null);
  }

  public deleteAssignment(assignmentId: number): Observable<any> {
    return this.httpService.delete(this.modulesController, `assignments/${assignmentId}`);
  }

  public downloadAssignmentFile(assignmentId: number, fileName: string): Observable<any> {
    return this.httpService.get(this.modulesController, `assignments/${assignmentId}/download?fileName=${fileName}`, null, true).pipe(
      tap(stream => saveAs(stream, fileName))
    );
  }
}
