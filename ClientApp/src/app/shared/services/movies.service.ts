import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';
import { TMDBSearchResults } from '../models/tmdb.interface';

@Injectable()
export class MoviesService {
  constructor(private apiService: ApiService) {}
}
