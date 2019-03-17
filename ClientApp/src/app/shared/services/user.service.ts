import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of, throwError, empty } from 'rxjs';
import { catchError, map, finalize } from 'rxjs/operators';

import { ConfigService } from './config.service';
import { BaseService } from './base.service';

@Injectable()
export class UserService extends BaseService {
  private baseUrl: string;

  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private _authTokenSource = new BehaviorSubject<string>(null);
  authToken$ = this._authTokenSource.asObservable();

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
          this._authTokenSource.next(data.auth_token);
          return true;
        })
      )
      .pipe(catchError(this.handleError));
  }

  logout(): Observable<boolean> {
    // This doesn't do anything currently
    return this.http
      .get(this.baseUrl + '/auth/accounts/logout')
      .pipe(map(data => true))
      .pipe(
        finalize(() => {
          localStorage.removeItem('auth_token');
          this.loggedIn = false;
          this._authNavStatusSource.next(false);
          this._authTokenSource.next(null);
        })
      )
      .pipe(
        catchError(err => {
          // we expect a 404 currently so this isnt a problem
          // it will still print an error in the conosle though
          if (err.status !== 404) return throwError(err);
          return of(true);
        })
      )
      .pipe(catchError(this.handleError));
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
