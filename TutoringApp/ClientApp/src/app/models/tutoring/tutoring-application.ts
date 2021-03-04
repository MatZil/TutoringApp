import { StudentCycleEnum } from '../enums/student-cycle-enum';
import { StudentYearEnum } from '../enums/student-year-enum';

export interface TutoringApplication {
  id: number;
  moduleName: string;
  studentName: string;
  email: string;
  studentCycle: StudentCycleEnum;
  studentYear: StudentYearEnum;
  faculty: string;
  studyBranch: string;
  requestDate: Date;
  motivationalLetter: string;

  studentCycleDisplay: string;
  studentYearDisplay: string;
}
