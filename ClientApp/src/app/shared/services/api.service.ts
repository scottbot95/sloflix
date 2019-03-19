import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { ConfigService } from './config.service';
import { BaseService } from './base.service';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';

export abstract class ApiService extends BaseService {
  private baseUrl: string;
  private httpOptions: object;

  constructor(
    protected http: HttpClient,
    protected configService: ConfigService
  ) {
    super();
    this.baseUrl = configService.getApiURI();

    const authToken = localStorage.getItem('auth_token');

    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authToken}`
      })
    };
  }

  protected get(url: string): Observable<any> {
    return this.http
      .get(this.baseUrl + url, this.httpOptions)
      .pipe(catchError(this.handleError));
  }

  protected post(url: string, body: any): Observable<any> {
    return this.http
      .post(this.baseUrl + url, body, this.httpOptions)
      .pipe(catchError(this.handleError));
  }
}
