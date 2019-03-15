import { Component, NO_ERRORS_SCHEMA } from '@angular/core';
import { Router } from '@angular/router';

import { UserRegistration } from '../../shared/models/user.registration.interface';
import { UserService } from '../../shared/services/user.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent {
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private userService: UserService, private router: Router) {}

  registerUser({ value, valid }: { value: UserRegistration; valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid) {
      this.userService.register(value.email, value.password).subscribe(
        result => {
          this.isRequesting = false;
          if (result) {
            this.router.navigate(['/login'], {
              queryParams: { brandNew: true, email: value.email }
            });
          }
        },
        errors => {
          this.isRequesting = false;
          this.errors = errors;
        }
      );
    }
  }
}
