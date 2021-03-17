import { StudentCycleEnum } from '../enums/student-cycle-enum';
import { StudentYearEnum } from '../enums/student-year-enum';

export interface Student {
  id: string;
  name: string;
  studentCycle: StudentCycleEnum;
  studentYear: StudentYearEnum;
  faculty: string;
  studyBranch: string;

  studentCycleDisplay: string;
  studentYearDisplay: string;
}
