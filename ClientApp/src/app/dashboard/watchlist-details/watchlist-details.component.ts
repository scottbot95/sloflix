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
import { MoviesService } from '../../shared/services/movies.service';
import { Movie } from 'src/app/shared/models/movie.interface';

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
    private watchlistService: WatchlistService,
    private movieService: MoviesService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.subscription = this.route.queryParamMap.subscribe(
      (params: ParamMap) => {
        this.watchlistService
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

    this.movieCards = this.watchlist.movies.map(m => {
      const card = <CardDetails>{
        title: m.title,
        image: m.posterPath,
        // content: m.summary,
        id: m.id,
        linkTo: '/dashboard/movie',
        queryParams: { id: m.id },
        rating: 0,
        setRating: this.setMovieRating
      };
      this.movieService.getMovieRating(m.id).subscribe(rating => {
        if (rating.myRating) {
          card.rating = rating.myRating;
        } else {
          card.rating = rating.avgRating || 0;
        }
      });
      return card;
    });
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
        const movie = <Movie>{
          title: result.title,
          posterPath: result.poster_path,
          tmdbId: result.id,
          summary: result.overview
        };
        this.movieService.createMovie(movie).subscribe(movie => {
          this.watchlistService
            .addMovieToWatchlist(this.watchlist.id, movie.id)
            .subscribe(watchlist => {
              this.watchlist = watchlist;
              this.generateMovieCards();
            });
        });
      }
    });
  }

  private setMovieRating = (movieId: number, rating: number) => {
    this.movieService.rateMovie(movieId, rating).subscribe(done => {
      console.log('rated movie!');
    });
  };
}
