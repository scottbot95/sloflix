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
import { Router } from '@angular/router';

@Injectable()
export class ApiService extends BaseService {
  private baseUrl: string;
  private httpOptions: { headers: HttpHeaders };

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private userService: UserService,
    private router: Router
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

    this.handleError = this.handleError.bind(this);
  }

  public get(url: string): Observable<any> {
    return this.http
      .get(this.baseUrl + url, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  public post(url: string, body: any): Observable<any> {
    return this.http
      .post(this.baseUrl + url, body, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  public delete(url: string): Observable<any> {
    return this.http
      .delete(url, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  protected handleError(error: HttpErrorResponse) {
    if (error.status === 401) {
      const authError = error.headers.get('www-authenticate');
      const queryParams = {
        redirectTo: this.router.url
      };
      if (authError.endsWith('"The token is expired"')) {
        queryParams['expired'] = true;
      }
      this.router.navigate(['/logout'], { queryParams });
      return super.handleError(error);
    } else {
      return throwError(error);
    }
  }
}
