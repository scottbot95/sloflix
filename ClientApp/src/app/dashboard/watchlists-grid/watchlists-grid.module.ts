import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WatchlistsGridComponent } from './watchlists-grid.component';
import { NewWatchlistDialogComponent } from './new-watchlist-dialog/new-watchlist-dialog.component';
import { MaterialModule } from 'src/app/shared/app.material.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { CardListModule } from 'src/app/shared/card-list/card-list.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule, MatInputModule } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    MaterialModule,
    CardListModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule
  ],
  declarations: [WatchlistsGridComponent, NewWatchlistDialogComponent],
  entryComponents: [NewWatchlistDialogComponent],
  exports: [WatchlistsGridComponent]
})
export class WatchlistsGridModule {}
