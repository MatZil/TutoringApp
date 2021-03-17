import { Component, Input, OnInit } from '@angular/core';
import { tap } from 'rxjs/operators';
import { ChatMessage } from 'src/app/models/chats/chat-message';
import { ChatMessageNew } from 'src/app/models/chats/chat-message-new';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ChatsService } from 'src/app/services/chats/chats.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  public chatMessages: ChatMessage[] = [];
  public currentUserId: string;

  public newMessage = '';

  @Input() private receiverId: string;

  constructor(
    private chatsService: ChatsService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initializeCurrentUserId();
    this.initializeChatMessages();
  }

  //#region Initialization
  private initializeCurrentUserId(): void {
    this.currentUserId = this.authService.getCurrentUserId();
  }

  private initializeChatMessages(): void {
    this.chatsService.getChatMessages(this.receiverId).pipe(
      tap(chatMessages => this.chatMessages = chatMessages)
    )
      .subscribe();
  }
  //#endregion

  //#region Event handlers
  public sendMessage(): void {
    const chatMessageNew: ChatMessageNew = { content: this.newMessage };

    this.chatsService.postChatMessage(this.receiverId, chatMessageNew).pipe(
      tap(_ => this.newMessage = '')
    )
    .subscribe();
  }
  //#endregion
}
