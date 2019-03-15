import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserService } from '../shared/services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  private subscription: Subscription;

  isExpanded: boolean = false;
  isLoggedIn: boolean = false;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.subscription = this.userService.authNavStatus$.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
