import { Observable, throwError } from 'rxjs';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { catchError } from 'rxjs/operators';

export abstract class BaseService {
  constructor() {}

  protected handleError(error: HttpErrorResponse) {
    if (error instanceof ErrorEvent) {
      // client-side/network error occurred
      return throwError(error.message);
    }
    console.warn(error);

    let applicationError = error.headers.get('Application-Error');

    if (applicationError) {
      return throwError(applicationError);
    }

    let modelStateErrors: string = '';
    let serverError = error.error;

    if (!serverError) {
      return throwError(error);
    }

    if (!serverError.type) {
      for (let key in serverError) {
        if (serverError.hasOwnProperty(key))
          modelStateErrors += serverError[key] + '\n';
      }
    }

    modelStateErrors = modelStateErrors === '' ? null : modelStateErrors;
    return throwError(modelStateErrors || 'Server error');
  }
}
