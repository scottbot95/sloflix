import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { ConfigService } from './config.service';
import { ApiService } from './api.service';

import {
  WatchlistSummary,
  WatchlistDetails
} from '../models/watchlist.interface';
import { tap, map } from 'rxjs/operators';
import { UserService } from './user.service';

@Injectable()
export class WatchlistService {
  // list of all watchlists
  private watchlistsSource: BehaviorSubject<WatchlistSummary[]>;
  public watchlists$: Observable<WatchlistSummary[]>;

  // currently selected watchlist
  private currentWatchlistSource: BehaviorSubject<WatchlistDetails>;
  public currentWatchlist$: Observable<WatchlistDetails>;

  constructor(private apiService: ApiService) {
    this.watchlistsSource = new BehaviorSubject<WatchlistSummary[]>(null);
    this.watchlists$ = this.watchlistsSource.asObservable();

    this.currentWatchlistSource = new BehaviorSubject<WatchlistDetails>(null);
    this.currentWatchlist$ = this.currentWatchlistSource.asObservable();
  }

  getAllWatchlists(): Observable<WatchlistSummary[]> {
    return this.apiService
      .get('/watchlists')
      .pipe(
        tap((data: WatchlistSummary[]) => this.watchlistsSource.next(data))
      );
  }

  getWatchlistDetails(id: number | string): Observable<WatchlistDetails> {
    return this.apiService
      .get(`/watchlists/${id}`)
      .pipe(
        tap((data: WatchlistDetails) => this.currentWatchlistSource.next(data))
      );
  }

  createWatchlist(watchlist: WatchlistSummary): Observable<WatchlistDetails> {
    return this.apiService
      .post('/watchlists', watchlist)
      .pipe(
        tap((data: WatchlistDetails) => this.currentWatchlistSource.next(data))
      );
  }

  deleteWatchlist(id: number | string): Observable<boolean> {
    return this.apiService
      .delete('/watchlists/' + id)
      .pipe(
        tap(data => {
          this.watchlistsSource.next(
            this.watchlistsSource.value.filter(wl => wl.id + '' !== id)
          );
        })
      )
      .pipe(map(data => true));
  }

  addMovieToWatchlist(
    watchlistId: number,
    movieId: number
  ): Observable<WatchlistDetails> {
    return this.apiService.put(`/watchlists/${watchlistId}/${movieId}`).pipe(
      tap(data => {
        this.currentWatchlistSource.next(data);
      })
    );
  }

  removeMovieFromWatchlist(
    watchlistId: number,
    movieId: number
  ): Observable<WatchlistDetails> {
    return this.apiService.delete(`/watchlists/${watchlistId}/${movieId}`).pipe(
      tap(data => {
        this.currentWatchlistSource.next(data);
      })
    );
  }
}
