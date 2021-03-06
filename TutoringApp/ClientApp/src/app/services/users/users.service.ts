import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IgnoredStudent } from 'src/app/models/users/ignored-student';
import { Student } from 'src/app/models/users/student';
import { Tutor } from 'src/app/models/users/tutor';
import { UserDto } from 'src/app/models/users/user-dto';
import { UserUnconfirmed } from 'src/app/models/users/user-unconfirmed';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private usersController = 'Users';

  constructor(
    private httpService: HttpService
  ) { }
  //#region Client side
  public getTutors(moduleId: number): Observable<Tutor[]> {
    return this.httpService.get(this.usersController, `tutors?moduleId=${moduleId}`);
  }

  public getStudents(moduleId: number): Observable<Student[]> {
    return this.httpService.get(this.usersController, `students?moduleId=${moduleId}`);
  }

  public ignoreStudent(studentId: string): Observable<any> {
    return this.httpService.post(this.usersController, `${studentId}/ignore`, null);
  }

  public unignoreStudent(studentId: string): Observable<any> {
    return this.httpService.post(this.usersController, `${studentId}/unignore`, null);
  }

  public getIgnoredStudents(): Observable<IgnoredStudent[]> {
    return this.httpService.get(this.usersController, 'ignored');
  }

  public getUser(userId: string): Observable<UserDto> {
    return this.httpService.get(this.usersController, userId);
  }

  public studentTutorExists(studentId: string, tutorId: string): Observable<boolean> {
    const queryParams = studentId
      ? `?studentId=${studentId}`
      : `?tutorId=${tutorId}`;

    return this.httpService.get(this.usersController, `student-tutor-exists${queryParams}`);
  }
  //#endregion

  //#region Admin side
  public getUnconfirmedUsers(): Observable<UserUnconfirmed[]> {
    return this.httpService.get(this.usersController, 'unconfirmed');
  }

  public confirmUser(id: string): Observable<any> {
    return this.httpService.post(this.usersController, `${id}/confirm`, null);
  }

  public rejectUser(id: string, rejectionReason: string): Observable<any> {
    return this.httpService.post(this.usersController, `${id}/reject?rejectionReason=${rejectionReason}`, null);
  }
  //#endregion
}
