import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType, HubConnectionState } from '@microsoft/signalr';
import { filter, tap } from 'rxjs/operators';
import { ChatMessage } from '../models/chats/chat-message';
import { TutoringSessionFinishedNotification } from '../models/tutoring/tutoring-sessions/tutoring-session-finished-notification';
import { TutoringSessionOnGoing } from '../models/tutoring/tutoring-sessions/tutoring-session-on-going';
import { TokenGetter } from '../utils/token-getter';
import { AuthService } from './auth/auth.service';
import { ChatsService } from './chats/chats.service';
import { TutoringSessionsService } from './tutoring/tutoring-sessions.service';
import { UrlService } from './url.service';

@Injectable({
  providedIn: 'root'
})
export class HubsService {
  private hubConnection: HubConnection;

  constructor(
    private authService: AuthService,
    private urlService: UrlService,
    private chatsService: ChatsService,
    private tutoringSessionsService: TutoringSessionsService
  ) { }

  public startConnections(): void {
    this.startMainHubConnection();
  }

  private startMainHubConnection(): void {
    this.authService.isAuthenticated$.pipe(
      filter(isAuth => isAuth),
      tap(_ => this.initalizeHubConnection())
    ).subscribe();
  }

  private async initalizeHubConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.urlService.getApiBaseUrl()}hubs/main`, {
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: TokenGetter
      })
      .build();

    if (this.hubConnection.state !== HubConnectionState.Connected) {
      this.hubConnection
        .start()
        .then(() => this.addChatMessageReceivedListener())
        .then(() => this.addTutoringSessionFinishedListener())
        .then(() => this.addOnGoingSessionListener())
        .catch(err => console.log(err));
    }
  }

  private addChatMessageReceivedListener(): void {
    this.hubConnection.on('chat-message-received', (chatMessage: ChatMessage) => {
      this.chatsService.updateReceivedChatMessage(chatMessage);
    });
  }

  private addTutoringSessionFinishedListener(): void {
    this.hubConnection.on('tutoring-session-finished', (notification: TutoringSessionFinishedNotification) => {
      this.tutoringSessionsService.finishTutoringSession(notification);
    });
  }

  private addOnGoingSessionListener(): void {
    this.hubConnection.on('tutoring-session-on-going', (notification: TutoringSessionOnGoing) => {
      this.tutoringSessionsService.setOngoingSession(notification);
    });
  }
}
