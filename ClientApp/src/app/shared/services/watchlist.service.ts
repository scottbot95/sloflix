import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { ConfigService } from './config.service';
import { ApiService } from './api.service';

import {
  WatchlistSummary,
  WatchlistDetails
} from '../models/watchlist.interface';
import { tap } from 'rxjs/operators';
import { UserService } from './user.service';

@Injectable()
export class WatchlistService extends ApiService {
  // list of all watchlists
  private watchlistsSource: BehaviorSubject<WatchlistSummary[]>;
  public watchlists$: Observable<WatchlistSummary[]>;

  // currently selected watchlist
  private currentWatchlistSource: BehaviorSubject<WatchlistDetails>;
  public currentWatchlist$: Observable<WatchlistDetails>;

  constructor(
    protected http: HttpClient,
    protected config: ConfigService,
    protected userService: UserService
  ) {
    super(http, config, userService);

    this.watchlistsSource = new BehaviorSubject<WatchlistSummary[]>(null);
    this.watchlists$ = this.watchlistsSource.asObservable();

    this.currentWatchlistSource = new BehaviorSubject<WatchlistDetails>(null);
    this.currentWatchlist$ = this.currentWatchlistSource.asObservable();
  }

  getAllWatchlists(): Observable<WatchlistSummary> {
    return this.get('/watchlists').pipe(
      tap(data => this.watchlistsSource.next(data))
    );
  }
}
