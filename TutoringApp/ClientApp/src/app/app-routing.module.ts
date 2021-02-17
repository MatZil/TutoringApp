import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppConstants } from './app.constants';
import { EmailConfirmationComponent } from './components/auth/email-confirmation/email-confirmation.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegistrationComponent } from './components/auth/registration/registration.component';


const routes: Routes = [
  { path: AppConstants.RegistrationRoute, component: RegistrationComponent },
  { path: AppConstants.LoginRoute, component: LoginComponent },
  { path: AppConstants.EmailConfirmationRoute, component: EmailConfirmationComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
