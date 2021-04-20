export interface TutoringSessionFinishedNotification {
  sessionId: number;
  tutorName: string;
  openNotificationDialog: boolean;
  participantId: string;
}
