import { Component, OnInit, OnDestroy } from '@angular/core';
import { Credentials } from '../../shared/models/credentials.interface';
import { UserService } from '../../shared/services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit, OnDestroy {
  private subscription: Subscription;

  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  credentials: Credentials = { email: '', password: '' };

  constructor(
    private userService: UserService,
    private rotuer: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.subscription = this.activatedRoute.queryParams.subscribe(params => {
      this.brandNew = params['brandNew'];
      this.credentials.email = params['email'];
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  loginUser({ value, valid }: { value: Credentials; valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid) {
      this.userService.login(value.email, value.password).subscribe(
        result => {
          this.isRequesting = false;
          if (result) {
            console.log('User successfully logged in');
          }
        },
        errors => {
          this.errors = errors;
          this.isRequesting = false;
        }
      );
    }
  }
}
