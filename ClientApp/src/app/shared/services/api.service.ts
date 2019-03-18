import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from '@angular/common/http';

import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { catchError } from 'rxjs/operators';
import { Observable, throwError, empty } from 'rxjs';
import { UserService } from './user.service';

export abstract class ApiService extends BaseService {
  private baseUrl: string;
  private httpOptions: { headers: HttpHeaders };

  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected userService: UserService
  ) {
    super();
    this.baseUrl = configService.getApiURI();

    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    this.userService.authToken$.subscribe(token => {
      if (token !== null) {
        this.httpOptions.headers = this.httpOptions.headers.set(
          'Authorization',
          `Bearer ${token}`
        );
      } else {
        this.httpOptions.headers = this.httpOptions.headers.delete(
          'Authorization'
        );
      }
    });

    this.handleAuthError = this.handleAuthError.bind(this);
  }

  protected get(url: string): Observable<any> {
    return this.http
      .get(this.baseUrl + url, this.httpOptions)
      .pipe(catchError(this.handleAuthError))
      .pipe(catchError(this.handleError));
  }

  protected post(url: string, body: any): Observable<any> {
    return this.http
      .post(this.baseUrl + url, body, this.httpOptions)
      .pipe(catchError(this.handleAuthError))
      .pipe(catchError(this.handleError));
  }

  private handleAuthError(error: HttpErrorResponse) {
    console.warn(error);
    if (error.status === 401) {
      this.userService.logout();
      return empty();
    } else {
      return throwError(error);
    }
  }
}
