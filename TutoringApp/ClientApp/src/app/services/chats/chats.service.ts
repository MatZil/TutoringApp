import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatMessage } from 'src/app/models/chats/chat-message';
import { ChatMessageNew } from 'src/app/models/chats/chat-message-new';
import { HttpService } from '../http.service';

@Injectable({
  providedIn: 'root'
})
export class ChatsService {
  private usersController = 'Users';

  constructor(
    private httpService: HttpService
  ) { }

  public postChatMessage(receiverId: string, chatMessage: ChatMessageNew): Observable<ChatMessage> {
    return this.httpService.post(this.usersController, `${receiverId}/chat-messages`, chatMessage);
  }

  public getChatMessages(receiverId: string, moduleId: number): Observable<ChatMessage[]> {
    return this.httpService.get(this.usersController, `${receiverId}/chat-messages?moduleId=${moduleId}`);
  }
}
