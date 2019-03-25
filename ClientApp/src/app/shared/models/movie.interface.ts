export interface Movie {
  id?: number;
  tmdbId?: number;
  title: string;
  summary: string;
  posterPath: string;
  rating?: MovieRating;
}

export interface MovieRating {
  avgRating: number;
  myRating: number;
}
