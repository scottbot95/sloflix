import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { ApiService } from './api.service';

@Injectable()
export class WatchlistService extends ApiService {
  constructor(protected http: HttpClient, protected config: ConfigService) {
    super(http, config);
  }
}
