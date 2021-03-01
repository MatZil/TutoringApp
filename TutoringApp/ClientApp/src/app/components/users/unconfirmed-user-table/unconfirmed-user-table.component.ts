import { Component, OnInit } from '@angular/core';
import { StudentCycleEnum } from 'src/app/models/enums/student-cycle-enum';
import { UserUnconfirmed } from 'src/app/models/users/user-unconfirmed';
import { UsersService } from 'src/app/services/users/users.service';
import { mapStudentYearToNumber } from 'src/app/utils/map-student-year-to-number';

@Component({
  selector: 'app-unconfirmed-user-table',
  templateUrl: './unconfirmed-user-table.component.html',
  styleUrls: ['./unconfirmed-user-table.component.scss']
})
export class UnconfirmedUserTableComponent implements OnInit {
  public users: UserUnconfirmed[] = [];
  public columns = [
    { field: 'name', header: 'Name'},
    { field: 'email', header: 'Email'},
    { field: 'faculty', header: 'Faculty'},
    { field: 'studyBranch', header: 'Branch'},
    { field: 'studentCycleDisplay', header: 'Cycle'},
    { field: 'studentYearDisplay', header: 'Year'}
  ];

  constructor(
    private usersService: UsersService
  ) { }

  ngOnInit(): void {
    this.initializeUsers();
  }

  //#region Initialization
  private initializeUsers(): void {
    this.usersService.getUnconfirmedUsers().subscribe(users => {
      this.users = users;
      this.users.map(user => {
        user.studentCycleDisplay = StudentCycleEnum[user.studentCycle];
        user.studentYearDisplay = mapStudentYearToNumber(user.studentYear).toString();
      });
    });
  }
  //#endregion
}
