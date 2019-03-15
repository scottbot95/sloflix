import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { ConfigService } from './config.service';
import { BaseService } from './base.service';

@Injectable()
export class UserService extends BaseService {
  private baseUrl: string;

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(
    protected http: HttpClient,
    protected configService: ConfigService
  ) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    this._authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getAuthURI();
  }

  register(email: string, password: string): Observable<boolean> {
    const body = { email, password };

    return this.http
      .post(this.baseUrl, body)
      .pipe(map(data => true))
      .pipe(catchError(this.handleError));
  }

  login(email: string, password: string): Observable<boolean> {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    const body = { email, password };
    return this.http
      .post<AuthenticationResponse>(this.baseUrl + '/login', body)
      .pipe(
        map((data: AuthenticationResponse) => {
          localStorage.setItem('auth_token', data.auth_token);
          this.loggedIn = true;
          this._authNavStatusSource.next(true);
          return true;
        })
      )
      .pipe(catchError(this.handleError));
  }

  logout(): Observable<boolean> {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
    this._authNavStatusSource.next(false);

    // This doesn't do anything currently
    return this.http
      .get(this.baseUrl + '/auth/accounts/logout')
      .pipe(map(data => true))
      .pipe(catchError(err => of(false)));
    // .pipe(catchError(this.handleError));
  }

  isLoggedIn(): boolean {
    return this.loggedIn;
  }
}

interface AuthenticationResponse {
  id: string;
  auth_token: string;
  expires_in: number;
}
