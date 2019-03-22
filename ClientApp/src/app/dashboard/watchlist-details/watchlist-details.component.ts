import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { of, Observable, Subscription } from 'rxjs';
import { WatchlistDetails } from '../../shared/models/watchlist.interface';
import { WatchlistService } from '../../shared/services/watchlist.service';
import { CardDetails } from '../../shared/components/card/card.component';
import { MatDialog } from '@angular/material';
import {
  AddMovieDialogComponent,
  AddMovieDialogData,
  AddMovieDialogResult
} from './add-movie-dialog/add-movie-dialog.component';

@Component({
  selector: 'app-watchlist-details',
  templateUrl: './watchlist-details.component.html',
  styleUrls: ['./watchlist-details.component.css']
})
export class WatchlistDetailsComponent implements OnInit {
  private subscription: Subscription;
  private watchlist: WatchlistDetails;
  private movieCards: CardDetails[];
  private isLoading: boolean = true;

  private editing: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private service: WatchlistService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.subscription = this.route.queryParamMap.subscribe(
      (params: ParamMap) => {
        this.service
          .getWatchlistDetails(params.get('id'))
          .subscribe(details => {
            this.watchlist = details;
            this.generateMovieCards();
            this.isLoading = false;
          });
      }
    );
  }

  private generateMovieCards() {
    if (!Array.isArray(this.watchlist.movies)) {
      this.movieCards = null;
      return;
    }

    this.movieCards = this.watchlist.movies.map(
      m =>
        <CardDetails>{
          title: m.title,
          image: m.posterPath,
          content: m.summary,
          id: m.id,
          linkTo: '/dashboard/movie',
          queryParams: { id: m.id }
        }
    );
  }

  private openDialog(): void {
    const dialogRef = this.dialog.open<
      AddMovieDialogComponent,
      AddMovieDialogData,
      AddMovieDialogResult
    >(AddMovieDialogComponent, {
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log(result);
      }
    });
  }
}
