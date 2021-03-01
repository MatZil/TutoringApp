import { StudentYearEnum } from '../models/enums/student-year-enum';

export function mapStudentYearToNumber(studentYear: StudentYearEnum): number {
  switch (studentYear) {
    case StudentYearEnum.FirstYear:
      return 1;
    case StudentYearEnum.SecondYear:
      return 2;
    case StudentYearEnum.ThirdYear:
      return 3;
    default:
      return 4;
  }
}
