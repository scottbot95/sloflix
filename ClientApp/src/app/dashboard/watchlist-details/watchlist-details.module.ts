import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WatchlistDetailsComponent } from './watchlist-details.component';
import { AddMovieDialogComponent } from './add-movie-dialog/add-movie-dialog.component';
import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../../shared/app.material.module';
import { CardListModule } from '../../shared/card-list/card-list.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatFormFieldModule, MatInputModule } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    MaterialModule,
    CardListModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
  declarations: [WatchlistDetailsComponent, AddMovieDialogComponent],
  entryComponents: [AddMovieDialogComponent],
  exports: [WatchlistDetailsComponent]
})
export class WatchlistDetailsModule {}
