import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { WatchlistSummary } from 'src/app/shared/models/watchlist.interface';

export type NewWatchlistDialogResult = WatchlistSummary;
export interface NewWatchlistDialogData {}

@Component({
  selector: 'app-new-watchlist-dialog',
  templateUrl: './new-watchlist-dialog.component.html',
  styleUrls: ['./new-watchlist-dialog.component.css']
})
export class NewWatchlistDialogComponent {
  data: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<
      NewWatchlistDialogComponent,
      NewWatchlistDialogResult
    >,
    @Inject(MAT_DIALOG_DATA) public dialogData: NewWatchlistDialogData,
    fb: FormBuilder
  ) {
    this.data = fb.group(<WatchlistSummary>{
      name: undefined
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  resetForm(): void {
    this.data.reset();
  }

  submitForm(event: Event): void {
    event.preventDefault();
    this.dialogRef.close(this.data.value);
  }
}
