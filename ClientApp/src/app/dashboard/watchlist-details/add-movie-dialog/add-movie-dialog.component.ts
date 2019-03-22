import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TmdbService } from '../../../shared/services/tmdb.service';
import { Observable } from 'rxjs';
import {
  startWith,
  map,
  debounceTime,
  distinctUntilChanged
} from 'rxjs/operators';
import { TMDBMovieSummary } from '../../../shared/models/tmdb.interface';

interface IAddMovieDialogResult {}
export type AddMovieDialogResult = IAddMovieDialogResult;
export interface AddMovieDialogData {}

@Component({
  selector: 'app-add-movie-dialog',
  templateUrl: './add-movie-dialog.component.html',
  styleUrls: ['./add-movie-dialog.component.css']
})
export class AddMovieDialogComponent implements OnInit {
  private data: FormGroup;
  private movies: string[] = ['Foobar', 'Foobar 2', 'Foobar 3'];
  private filteredOptions: Observable<string[]>;
  private options: TMDBMovieSummary[] = [
    { title: 'Terminator', id: 1 },
    { title: 'Terminator 2', id: 3 },
    { title: 'Terminator', id: 57 }
  ];

  constructor(
    public dialogRef: MatDialogRef<
      AddMovieDialogComponent,
      AddMovieDialogResult
    >,
    @Inject(MAT_DIALOG_DATA) public dialogData: AddMovieDialogData,
    fb: FormBuilder,
    private tmdb: TmdbService
  ) {
    this.data = fb.group({
      movieTitle: undefined
    });
  }

  ngOnInit(): void {
    this.filteredOptions = this.data.valueChanges.pipe(
      // startWith<string | TMDBMovieSummary>(''),
      // debounceTime(400),
      // distinctUntilChanged(),
      // map(value => (typeof value === 'string' ? value : value.title)),
      // map(title => (title ? this._filter(title) : this.options.slice()))
      startWith(''),
      map(title => this._filter(title))
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  displayFn(movie?: TMDBMovieSummary): string | undefined {
    return movie ? movie.title : undefined;
  }

  private submitForm(event: Event): void {
    event.preventDefault();
    this.tmdb.searchMovies(this.data.value['movieTitle']);
  }

  private _filter(query: string): string[] {
    query = query ? query.toLowerCase() : '';
    if (query && query.length > 3) {
      console.log(query);
    } else {
    }
    // return [];
    return this.movies.filter(opt => opt.startsWith(query));
  }
}
