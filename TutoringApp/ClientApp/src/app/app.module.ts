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

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    LoginComponent,
    ToolbarComponent,
    EmailConfirmationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,

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
  ],
  providers: [
    { provide: LocationStrategy, useClass: PathLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
