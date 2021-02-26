import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ButtonModule } from 'primeng/button';
import { HttpClientModule } from '@angular/common/http';
import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { RegistrationComponent } from './components/auth/registration/registration.component';
import { LoginComponent } from './components/auth/login/login.component';
import { JwtModule } from '@auth0/angular-jwt';
import { TokenGetter } from './utils/token-getter';
import { ToolbarModule } from 'primeng/toolbar';
import { ToolbarComponent } from './components/shared/toolbar/toolbar.component';
import { MenuModule } from 'primeng/menu';
import { EmailConfirmationComponent } from './components/auth/email-confirmation/email-confirmation.component';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessageModule } from 'primeng/message';
import { MessagesModule } from 'primeng/messages';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './components/shared/home/home.component';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { ModuleListComponent } from './components/modules/module-list/module-list.component';
import { VirtualScrollerModule } from 'primeng/virtualscroller';
import { ModuleViewComponent } from './components/modules/module-view/module-view.component';
import { ModuleCreateComponent } from './components/modules/module-create/module-create.component';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    LoginComponent,
    ToolbarComponent,
    EmailConfirmationComponent,
    HomeComponent,
    ModuleListComponent,
    ModuleViewComponent,
    ModuleCreateComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,

    // JWT Management
    JwtModule.forRoot({
      config: {
        tokenGetter: TokenGetter,
        allowedDomains: ['https://localhost:5001']
      }
    }),

    // PrimeNG components
    ButtonModule,
    ToolbarModule,
    MenuModule,
    ProgressSpinnerModule,
    MessagesModule,
    MessageModule,
    InputTextModule,
    ToastModule,
    VirtualScrollerModule,
    OverlayPanelModule,
    ConfirmDialogModule,
  ],
  providers: [
    { provide: LocationStrategy, useClass: PathLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
