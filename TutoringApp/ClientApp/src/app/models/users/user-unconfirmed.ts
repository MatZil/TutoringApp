import { StudentCycleEnum } from '../enums/student-cycle-enum';
import { StudentYearEnum } from '../enums/student-year-enum';

export interface UserUnconfirmed {
  id: string;
  name: string;
  email: string;
  studentCycle: StudentCycleEnum;
  studentYear: StudentYearEnum;
  faculty: string;
  studyBranch: string;
}
