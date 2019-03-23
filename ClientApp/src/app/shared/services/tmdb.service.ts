import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { Observable, throwError } from 'rxjs';
import { TMDBSearchResults } from '../models/tmdb.interface';
import { ApiService } from './api.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';

interface TMDbConfiguration {
  images: {
    base_url: string;
    secure_base_url: string;
    poster_sizes: string[];
  };
}

@Injectable()
export class TmdbService {
  private _config: TMDbConfiguration;

  constructor(private http: HttpClient) {
    const configURL = environment.tmdbBaseURI + '/configuration';
    let params = this.getBaseParams();
    http.get<TMDbConfiguration>(configURL, { params }).subscribe(result => {
      this._config = result;
    });
  }

  searchMovies(query: string): Observable<TMDBSearchResults> {
    const url = environment.tmdbBaseURI + '/search/movie';
    let params = this.getBaseParams();
    params = params.append('query', query);
    return this.http.get<TMDBSearchResults>(url, { params });
    // .pipe(catchError(this.handleError));
  }

  private getBaseParams(): HttpParams {
    const params = new HttpParams();
    return params.append('api_key', environment.tmdbPublicKey);
  }

  private handleError(error: any): any {
    return throwError(error);
  }
}
