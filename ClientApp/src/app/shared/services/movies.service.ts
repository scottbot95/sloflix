import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';
import { TMDBSearchResults } from '../models/tmdb.interface';
import { Movie, MovieRating } from '../models/movie.interface';

@Injectable()
export class MoviesService {
  constructor(private apiService: ApiService) {}

  public createMovie(movie: Movie): Observable<Movie> {
    return this.apiService.post('/movies', movie);
  }

  public getMovieRating(movieId: number): Observable<MovieRating> {
    return this.apiService.get(`/movies/${movieId}/rating`);
  }

  public rateMovie(movieId: number, rating: number): Observable<boolean> {
    return this.apiService.put(`/movies/${movieId}/rating`, { rating });
  }
}
