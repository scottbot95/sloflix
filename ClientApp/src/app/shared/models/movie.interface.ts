export interface Movie {
  id?: number;
  tmdbId?: number;
  title: string;
  summary: string;
  posterPath: string;
}

export interface MovieRating {
  avgRating: number;
  myRating: number;
}
