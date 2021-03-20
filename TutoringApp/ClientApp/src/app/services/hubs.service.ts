import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { filter, tap } from 'rxjs/operators';
import { ChatMessage } from '../models/chats/chat-message';
import { TokenGetter } from '../utils/token-getter';
import { AuthService } from './auth/auth.service';
import { ChatsService } from './chats/chats.service';
import { UrlService } from './url.service';

@Injectable({
  providedIn: 'root'
})
export class HubsService {
  private hubConnection: HubConnection;

  constructor(
    private authService: AuthService,
    private urlService: UrlService,
    private chatsService: ChatsService
  ) { }

  public startConnections(): void {
    this.startMainHubConnection();
  }

  private startMainHubConnection(): void {
    this.authService.isAuthenticated$.pipe(
      filter(isAuth => isAuth),
      tap(_ => console.log('User Authenticated: Initializing SignalR...')),
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
        .catch(err => console.log(err));
    }
  }

  private addChatMessageReceivedListener(): void {
    console.log('Opening Chat Listener...');

    this.hubConnection.on('chat-message-received', (chatMessage: ChatMessage) => {
      this.chatsService.updateReceivedChatMessage(chatMessage);
    });

    console.log('Chat Listener Opened.');
  }
}
