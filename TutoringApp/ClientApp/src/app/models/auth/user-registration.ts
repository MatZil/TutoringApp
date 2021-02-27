import { StudentCycleEnum } from '../enums/student-cycle-enum';
import { StudentYearEnum } from '../enums/student-year-enum';
import { UserLogin } from './user-login';

export class UserRegistration extends UserLogin {
    firstName: string;
    lastName: string;
    studentCycle: StudentCycleEnum;
    studentYear: StudentYearEnum;
    faculty: string;
    studyBranch: string;
}
