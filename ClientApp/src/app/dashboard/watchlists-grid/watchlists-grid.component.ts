import { Component, AfterContentInit } from '@angular/core';
import { WatchlistSummary } from '../../shared/models/watchlist.interface';
import { WatchlistService } from '../../shared/services/watchlist.service';
import { CardDetails } from '../../shared/components/card/card.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-watchlists-grid',
  templateUrl: './watchlists-grid.component.html',
  styleUrls: ['./watchlists-grid.component.css']
})
export class WatchlistsGridComponent implements AfterContentInit {
  private watchlists: WatchlistSummary[];
  private cardItems: CardDetails[];

  constructor(
    private watchlistService: WatchlistService,
    private router: Router
  ) {}

  ngAfterContentInit() {
    this.loadData();
  }

  loadData() {
    this.watchlistService.getAllWatchlists().subscribe(data => {
      this.watchlistService.watchlists$.subscribe(summaries => {
        this.watchlists = summaries;
        this.cardItems = summaries.map(
          s =>
            <CardDetails>{
              id: s.id + '',
              title: s.name,
              onClick: this.handleCardClick
            }
        );
      });
    });
  }

  private handleCardClick = (id: string) => {
    this.router.navigate([`/dashboard/watchlist/${id}`]);
  };
}
