import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { filter, tap } from 'rxjs/operators';
import { ChatMessage } from '../models/chats/chat-message';
import { AuthService } from './auth/auth.service';
import { ChatsService } from './chats/chats.service';
import { UrlService } from './url.service';

@Injectable({
  providedIn: 'root'
})
export class HubsService {
  private hubConnection: signalR.HubConnection;

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
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.urlService.getApiBaseUrl()}hubs/main`, {
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      await this.startHubConnection(this.hubConnection);
    }
  }

  private async startHubConnection(connection: signalR.HubConnection) {
    console.log('Starting SignalR Connection...');
    try {
      await connection.start()
      console.log('SignalR Connected.');
      this.addChatMessageReceivedListener();
    } catch (err) {
      console.assert(connection.state === signalR.HubConnectionState.Disconnected);
      console.log(err);
      setTimeout(() => this.startHubConnection(connection), 5000);
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
