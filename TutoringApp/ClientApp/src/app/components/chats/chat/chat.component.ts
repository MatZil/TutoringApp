import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChatMessage } from 'src/app/models/chats/chat-message';
import { ChatsService } from 'src/app/services/chats/chats.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  public chatMessages: ChatMessage[] = [];

  constructor(
    private chatsService: ChatsService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
  }

  //#region Initialization

  //#endregion
}
