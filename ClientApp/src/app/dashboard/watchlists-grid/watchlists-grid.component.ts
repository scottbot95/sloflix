import { Component, AfterContentInit } from '@angular/core';
import { WatchlistSummary } from '../../shared/models/watchlist.interface';
import { WatchlistService } from '../../shared/services/watchlist.service';
import { CardDetails } from '../../shared/components/card/card.component';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import {
  NewWatchlistDialogComponent,
  NewWatchlistDialogData,
  NewWatchlistDialogResult
} from './new-watchlist-dialog/new-watchlist-dialog.component';

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
    private router: Router,
    private dialog: MatDialog
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
              id: s.id,
              title: s.name,
              linkTo: '/dashboard/watchlist',
              queryParams: { id: s.id },
              actions: [
                {
                  action: this.handleEdit,
                  label: 'edit',
                  isIcon: true
                }
              ]
            }
        );
      });
    });
  }

  private handleEdit = (id: string) => {
    this.router.navigate([`/dashboard/watchlist`], {
      queryParams: { id, edit: true }
    });
  };

  private openDialog(): void {
    const dialogRef = this.dialog.open<
      NewWatchlistDialogComponent,
      NewWatchlistDialogData,
      NewWatchlistDialogResult
    >(NewWatchlistDialogComponent, {
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.watchlistService.createWatchlist(result).subscribe(created => {
          this.router.navigate(['/dashboard/watchlist'], {
            queryParams: { id: created.id }
          });
        });
      }
    });
  }
}
