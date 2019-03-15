import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
  private _apiURI: string;
  private _authURI: string;

  constructor() {
    this._apiURI = 'https://localhost:5001/api';
    this._authURI = 'https://localhost:5001/auth/accounts';
  }

  getApiURI() {
    return this._apiURI;
  }

  getAuthURI() {
    return this._authURI;
  }
}
