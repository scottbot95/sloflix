import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

interface INewWatchlistDialogResult {}

export type NewWatchlistDialogResult = INewWatchlistDialogResult;
export interface NewWatchlistDialogData {}

@Component({
  selector: 'app-new-watchlist-dialog',
  templateUrl: './new-watchlist-dialog.component.html',
  styleUrls: ['./new-watchlist-dialog.component.css']
})
export class NewWatchlistDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<
      NewWatchlistDialogComponent,
      NewWatchlistDialogResult
    >,
    @Inject(MAT_DIALOG_DATA) public data: NewWatchlistDialogData
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
