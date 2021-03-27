import { TutoringSessionEvaluationEnum } from '../../enums/tutoring-session-evaluation-enum';

export interface TutoringSessionEvaluation {
  evaluation: TutoringSessionEvaluationEnum;
  comment: string;
}
