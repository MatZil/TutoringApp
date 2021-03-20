import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth/auth.service';
import { HubsService } from './services/hubs.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private hubsService: HubsService
  ) { }

  ngOnInit(): void {
    this.preserveAuthenticationState();

    this.hubsService.startConnections();
  }

  private preserveAuthenticationState(): void {
    const isCurrentlyAuthenticated = this.authService.isCurrentlyAuthenticated();

    if (isCurrentlyAuthenticated) {
      this.authService.changeAuthState(true);
    }
  }
}
