import { StudentCycleEnum } from '../enums/student-cycle-enum';
import { StudentYearEnum } from '../enums/student-year-enum';

export interface Tutor {
  id: string;
  name: string;
  studentCycle: StudentCycleEnum;
  studentYear: StudentYearEnum;
  faculty: string;
  studyBranch: string;
  averageScore: number;
  tutoringSessionCount: number;

  studentCycleDisplay: string;
  studentYearDisplay: string;
}
