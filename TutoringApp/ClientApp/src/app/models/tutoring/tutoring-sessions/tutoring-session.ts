import { TutoringSessionEvaluationEnum } from '../../enums/tutoring-session-evaluation-enum';
import { TutoringSessionStatusEnum } from '../../enums/tutoring-session-status-enum';

export interface TutoringSession {
  id: number;
  moduleName: string;
  participantName: string;
  creationDate: Date;
  isSubscribed: boolean;
  sessionDate: Date;
  status: TutoringSessionStatusEnum;
  statusChangeDate: Date;
  evaluation: TutoringSessionEvaluationEnum;

  statusDisplay: string;
  evaluationDisplay: string;
  isActive: boolean;
}
