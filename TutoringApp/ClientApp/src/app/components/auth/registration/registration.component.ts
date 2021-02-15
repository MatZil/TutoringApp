import { Component, OnInit } from '@angular/core';
import { UserRegistration } from 'src/app/models/auth/user-registration';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {
  }

  public register() {
    const userRegistration: UserRegistration = {
      firstName: ' Matas',
      lastName: 'Zilinskas',
      email: 'zilinskas.matas1999@gmail.com',
      password: 'sixlet'
    };

    this.authService.register(userRegistration).subscribe();
  }
}
