import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserService } from '../../shared/services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit, OnDestroy {
  private subscription: Subscription;

  success: boolean = false;
  isRequesting: boolean = true;
  complete: boolean = false;
  errors: string;

  constructor(
    private userService: UserService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.subscription = this.activatedRoute.queryParams.subscribe(params => {
      if (params['expired']) this.errors = 'Your session expired';
    });
    this.logout();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  logout() {
    this.isRequesting = true;
    this.userService
      .logout()
      .pipe(
        finalize(() => {
          this.isRequesting = false;
        })
      )
      .subscribe(
        success => {
          this.success = true;
        },
        errors => {
          this.errors = errors;
          this.success = false;
        }
      );
  }
}
