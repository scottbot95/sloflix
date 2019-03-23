export interface PagedResult<T> {
  page: number;
  total_results: number;
  total_pages: number;
  results: T[];
}

export interface TMDBMovieSummary {
  id: number;
  poster_path?: string;
  title: string;
  overview: string;
}

export type TMDBSearchResults = PagedResult<TMDBMovieSummary>;
