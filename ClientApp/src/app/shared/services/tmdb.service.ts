import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { Observable, throwError } from 'rxjs';
import { TMDBSearchResults } from '../models/tmdb.interface';
import { ApiService } from './api.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';

@Injectable()
export class TmdbService {
  constructor(private http: HttpClient) {}

  searchMovies(query: string): Observable<TMDBSearchResults> {
    const url = environment.tmdbBaseURI + '/search/movie';
    let params = this.getBaseParams();
    params = params.append('query', query);
    this.http
      .get<TMDBSearchResults>(url, { params })
      .pipe(catchError(this.handleError))
      .subscribe(result => console.log(result));
    return null;
  }

  private getBaseParams(): HttpParams {
    const params = new HttpParams();
    return params.append('api_key', environment.tmdbPublicKey);
  }

  private handleError(error: any): any {
    return throwError(error);
  }
}
