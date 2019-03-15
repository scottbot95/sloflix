import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
  success: boolean = false;
  errors: string;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit() {
    this.logout();
  }

  logout() {
    this.userService.logout().subscribe(
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
