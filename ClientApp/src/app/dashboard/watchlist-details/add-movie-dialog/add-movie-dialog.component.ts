import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

interface IAddMovieDialogResult {}
export type AddMovieDialogResult = IAddMovieDialogResult;
export interface AddMovieDialogData {}

@Component({
  selector: 'app-add-movie-dialog',
  templateUrl: './add-movie-dialog.component.html',
  styleUrls: ['./add-movie-dialog.component.css']
})
export class AddMovieDialogComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<
      AddMovieDialogComponent,
      AddMovieDialogResult
    >
  ) {}

  ngOnInit() {}
}
