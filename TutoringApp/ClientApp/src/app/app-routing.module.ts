import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppConstants } from './app.constants';
import { AuthGuard } from './auth.guard';
import { EmailConfirmationComponent } from './components/auth/email-confirmation/email-confirmation.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegistrationComponent } from './components/auth/registration/registration.component';
import { ModuleViewComponent } from './components/modules/module-view/module-view.component';
import { HomeComponent } from './components/shared/home/home.component';
import { TutorViewComponent } from './components/users/tutor-view/tutor-view.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: AppConstants.RegistrationRoute, component: RegistrationComponent },
  { path: AppConstants.LoginRoute, component: LoginComponent },
  { path: AppConstants.EmailConfirmationRoute, component: EmailConfirmationComponent },
  { path: AppConstants.ModuleViewRoute, component: ModuleViewComponent, canActivate: [AuthGuard] },
  { path: AppConstants.TutorViewRoute, component: TutorViewComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
