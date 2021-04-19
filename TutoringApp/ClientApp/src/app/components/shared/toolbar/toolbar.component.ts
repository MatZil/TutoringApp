import { Component, OnInit, ViewChild } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { filter, map, switchMap, tap } from 'rxjs/operators';
import { AppConstants } from 'src/app/app.constants';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModulesService } from 'src/app/services/modules/modules.service';
import { UserIgnoresSidebarComponent } from '../../users/user-ignores-sidebar/user-ignores-sidebar.component';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {
  public userMenuItems: MenuItem[];

  public moduleName = '';
  public moduleId: number;

  @ViewChild(UserIgnoresSidebarComponent)
  private userIgnoresSidebarComponent: UserIgnoresSidebarComponent;

  constructor(
    private authService: AuthService,
    private router: Router,
    private modulesService: ModulesService
  ) { }

  ngOnInit(): void {
    this.initializeIsAuthenticatedSubscription();
    this.initializeRouteChangeSubscription();
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

  private initializeRouteChangeSubscription(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(event => (event as NavigationEnd).url),
      map(url => url.split('/').slice(1, 3)),
      tap(urlSegments => this.resetModuleName(urlSegments[0])),
      filter(urlSegments => urlSegments[0] === 'modules'),
      map(urlSegments => +urlSegments[1]),
      filter(moduleId => moduleId !== this.moduleId),
      tap(moduleId => this.moduleId = moduleId),
      switchMap(moduleId => this.modulesService.getModule(moduleId)),
      tap(module => this.moduleName = module.name)
    )
      .subscribe();
  }

  private resetModuleName(urlStart: string): void {
    if (urlStart !== 'modules') {
      this.moduleName = '';
      this.moduleId = null;
    }
  }

  private initializeAuthenticatedItems(): void {
    this.userMenuItems = [
      { label: 'Ignore List', icon: 'pi pi-ban', command: _ => this.userIgnoresSidebarComponent.openSidebar() },
      { label: 'Logout', icon: 'pi pi-sign-out', command: _ => this.logout() }
    ];
  }

  private initializeUnauthenticatedItems(): void {
    this.userMenuItems = [
      { label: 'Register', icon: 'pi pi-check-square', routerLink: AppConstants.RegistrationRoute },
      { label: 'Login', icon: 'pi pi-sign-in', routerLink: AppConstants.LoginRoute }
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
