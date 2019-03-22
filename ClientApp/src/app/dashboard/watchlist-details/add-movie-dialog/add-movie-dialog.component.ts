import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TmdbService } from '../../../shared/services/tmdb.service';
import { Observable, of } from 'rxjs';
import {
  startWith,
  map,
  debounceTime,
  distinctUntilChanged,
  switchMap
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
  private filteredOptions: Observable<TMDBMovieSummary[]>;
  private options: TMDBMovieSummary[] = [
    { title: 'Terminator', id: 1 },
    { title: 'Terminator 2', id: 3 },
    { title: 'Foobar', id: 57 }
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
      movie: undefined
    });
  }

  ngOnInit(): void {
    this.filteredOptions = this.data.valueChanges.pipe(
      map(data => data['movie']),
      debounceTime(250),
      distinctUntilChanged(),
      startWith<string | TMDBMovieSummary>(''),
      map(value => (typeof value === 'string' ? value : value.title)),
      switchMap(query => (query ? this.searchMovies(query) : of([])))
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  displayFn(movie?: TMDBMovieSummary): string | undefined {
    return movie ? movie.title : undefined;
  }

  private searchMovies(query: string): Observable<TMDBMovieSummary[]> {
    return this.tmdb.searchMovies(query).pipe(map(result => result.results));
  }

  private submitForm(event: Event): void {
    event.preventDefault();
    this.tmdb.searchMovies(this.data.value['movie']);
  }

  private _filter(query: string): TMDBMovieSummary[] {
    query = query ? query.toLowerCase() : '';
    if (query && query.length > 3) {
      console.log(query);
    } else {
    }
    // return [];
    return this.options.filter(opt =>
      opt.title.toLowerCase().startsWith(query)
    );
  }
}
