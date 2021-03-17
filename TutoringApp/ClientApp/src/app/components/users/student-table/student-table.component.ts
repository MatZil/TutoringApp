import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StudentCycleEnum } from 'src/app/models/enums/student-cycle-enum';
import { Student } from 'src/app/models/users/student';
import { UsersService } from 'src/app/services/users/users.service';
import { mapStudentYearToNumber } from 'src/app/utils/map-student-year-to-number';

@Component({
  selector: 'app-student-table',
  templateUrl: './student-table.component.html',
  styleUrls: ['./student-table.component.scss']
})
export class StudentTableComponent implements OnInit {
  public students: Student[] = [];
  public columns = [
    { field: 'name', header: 'Name' },
    { field: 'faculty', header: 'Faculty' },
    { field: 'studyBranch', header: 'Branch' },
    { field: 'studentCycleDisplay', header: 'Cycle' },
    { field: 'studentYearDisplay', header: 'Year' }
  ];

  private moduleId: number;

  constructor(
    private usersService: UsersService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeModuleId();
    this.initializeStudents();
  }

  //#region Initialization
  private initializeModuleId(): void {
    this.activatedRoute.params.subscribe(params => {
      this.moduleId = +params.id;
    });
  }

  private initializeStudents(): void {
    this.usersService.getStudents(this.moduleId).subscribe(students => {
      this.students = students;
      this.students.map(student => {
        student.studentCycleDisplay = StudentCycleEnum[student.studentCycle];
        student.studentYearDisplay = mapStudentYearToNumber(student.studentYear).toString();
      });
    });
  }
  //#endregion

  //#region Event handlers
  public navigateToStudentView(studentId: string): void {
    this.router.navigateByUrl(`students/${studentId}`);
  }
  //#endregion
}
