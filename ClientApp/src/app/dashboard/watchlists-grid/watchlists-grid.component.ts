import { Component, OnInit } from '@angular/core';
import { WatchlistSummary } from '../../shared/models/watchlist.interface';
import { WatchlistService } from '../../shared/services/watchlist.service';

@Component({
  selector: 'app-watchlists-grid',
  templateUrl: './watchlists-grid.component.html',
  styleUrls: ['./watchlists-grid.component.css']
})
export class WatchlistsGridComponent implements OnInit {
  public watchlists: WatchlistSummary[];

  constructor(private watchlistService: WatchlistService) {}

  ngOnInit() {
    this.watchlistService.getAllWatchlists().subscribe(data => {
      this.watchlistService.watchlists$.subscribe(summaries => {
        this.watchlists = summaries;
      });
    });
  }
}
