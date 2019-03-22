import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import {
  MatFormFieldModule,
  MatInputModule,
  MatAutocompleteModule
} from '@angular/material';
import { WatchlistDetailsComponent } from './watchlist-details.component';
import { AddMovieDialogComponent } from './add-movie-dialog/add-movie-dialog.component';
import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../../shared/app.material.module';
import { CardListModule } from '../../shared/card-list/card-list.module';
import { TmdbService } from '../../shared/services/tmdb.service';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    MaterialModule,
    CardListModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    FormsModule
  ],
  declarations: [WatchlistDetailsComponent, AddMovieDialogComponent],
  entryComponents: [AddMovieDialogComponent],
  exports: [WatchlistDetailsComponent],
  providers: [TmdbService]
})
export class WatchlistDetailsModule {}
