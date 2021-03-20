import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { VirtualScroller } from 'primeng/virtualscroller';
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
export class ChatComponent implements OnInit, AfterViewInit {
  public chatMessages: ChatMessage[] = [];
  public currentUserId: string;

  public newMessage = '';

  @Input() private receiverId: string;
  @Input() private moduleId: number;

  @ViewChild(VirtualScroller)
  private virtualScrollerComponent: VirtualScroller;

  constructor(
    private chatsService: ChatsService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initializeCurrentUserId();
  }

  ngAfterViewInit(): void {
    this.initializeChatMessages();
  }

  //#region Initialization
  private initializeCurrentUserId(): void {
    this.currentUserId = this.authService.getCurrentUserId();
  }

  private initializeChatMessages(): void {
    this.chatsService.getChatMessages(this.receiverId, this.moduleId).pipe(
      tap(chatMessages => this.chatMessages = chatMessages),
      tap(_ => this.scrollToBottom())
    )
      .subscribe();
  }

  private scrollToBottom(): void {
    setTimeout(() => this.virtualScrollerComponent.viewport.scrollTo({ bottom: 0 }));
  }
  //#endregion

  //#region Event handlers
  public sendMessage(): void {
    const chatMessageNew: ChatMessageNew = { content: this.newMessage, moduleId: this.moduleId };

    this.chatsService.postChatMessage(this.receiverId, chatMessageNew).pipe(
      tap(_ => this.newMessage = '')
    )
      .subscribe();
  }
  //#endregion
}
