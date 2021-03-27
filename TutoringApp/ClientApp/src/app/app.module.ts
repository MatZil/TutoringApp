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
import { DropdownModule } from 'primeng/dropdown';
import { UnconfirmedUserTableComponent } from './components/users/unconfirmed-user-table/unconfirmed-user-table.component';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DialogModule } from 'primeng/dialog';
import { UserRejectionDialogComponent } from './components/users/user-rejection-dialog/user-rejection-dialog.component';
import { ModuleTutorApplicationDialogComponent } from './components/modules/module-tutor-application-dialog/module-tutor-application-dialog.component';
import { TutoringApplicationTableComponent } from './components/tutoring/tutoring-application-table/tutoring-application-table.component';
import { TutorTableComponent } from './components/users/tutor-table/tutor-table.component';
import { CheckboxModule } from 'primeng/checkbox';
import { ChatComponent } from './components/chats/chat/chat.component';
import { TutorViewComponent } from './components/users/tutor-view/tutor-view.component';
import { StudentTableComponent } from './components/users/student-table/student-table.component';
import { StudentViewComponent } from './components/users/student-view/student-view.component';
import { TutoringSessionTableComponent } from './components/tutoring/tutoring-session-table/tutoring-session-table.component';

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
    ModuleCreateComponent,
    UnconfirmedUserTableComponent,
    UserRejectionDialogComponent,
    ModuleTutorApplicationDialogComponent,
    TutoringApplicationTableComponent,
    TutorTableComponent,
    ChatComponent,
    TutorViewComponent,
    StudentTableComponent,
    StudentViewComponent,
    TutoringSessionTableComponent,
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
    DropdownModule,
    TableModule,
    TabViewModule,
    InputTextareaModule,
    DialogModule,
    CheckboxModule,
  ],
  providers: [
    { provide: LocationStrategy, useClass: PathLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
