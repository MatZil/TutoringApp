import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AppConstants } from 'src/app/app.constants';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {
  public userMenuItems: MenuItem[];

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeIsAuthenticatedSubscription();
  }

  //#region Initialization
  private initializeIsAuthenticatedSubscription(): void {
    this.authService.isAuthenticated$.subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.initializeAuthenticatedItems();
      } else {
        this.initializeUnauthenticatedItems();
      }
    });
  }

  private initializeAuthenticatedItems(): void {
    this.userMenuItems = [
      { label: 'Atsijungti', icon: 'pi pi-sign-out', command: _ => this.logout() }
    ];
  }

  private initializeUnauthenticatedItems(): void {
    this.userMenuItems = [
      { label: 'Užsiregistruoti', icon: 'pi pi-check-square', routerLink: AppConstants.RegistrationRoute },
      { label: 'Prisijungti', icon: 'pi pi-sign-in', routerLink: AppConstants.LoginRoute }
    ];
  }
  //#endregion

  //#region Event handlers
  private logout(): void {
    this.authService.logout();
    this.navigateHome();
  }

  public navigateHome(): void {
    this.router.navigateByUrl('');
  }
  //#endregion
}
