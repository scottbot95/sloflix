import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
  private _apiURI: string;
  private _authURI: string;

  constructor() {
    const baseURI = 'https://localhost:5001';
    this._apiURI = `${baseURI}/api`;
    this._authURI = `${baseURI}/auth/accounts`;
  }

  getApiURI() {
    return this._apiURI;
  }

  getAuthURI() {
    return this._authURI;
  }
}
