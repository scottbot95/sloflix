import { Movie } from './movie.interface';

export interface WatchlistSummary {
  id?: number;
  name: string;
  movies?: number[];
}

export interface WatchlistDetails {
  id?: number;
  name: string;
  movies: Movie[];
}
