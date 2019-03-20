import { Component, AfterContentInit } from '@angular/core';
import { WatchlistSummary } from '../../shared/models/watchlist.interface';
import { WatchlistService } from '../../shared/services/watchlist.service';

@Component({
  selector: 'app-watchlists-grid',
  templateUrl: './watchlists-grid.component.html',
  styleUrls: ['./watchlists-grid.component.css']
})
export class WatchlistsGridComponent implements AfterContentInit {
  public watchlists: WatchlistSummary[];

  constructor(private watchlistService: WatchlistService) {}

  ngAfterContentInit() {
    this.loadData();
  }

  loadData() {
    this.watchlistService.getAllWatchlists().subscribe(data => {
      this.watchlistService.watchlists$.subscribe(summaries => {
        this.watchlists = summaries;
      });
    });
  }
}
