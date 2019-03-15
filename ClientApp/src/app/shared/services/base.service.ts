import { Observable, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

export abstract class BaseService {
  constructor() {}

  protected handleError(error: HttpErrorResponse) {
    if (error instanceof ErrorEvent) {
      // client-side/network error occurred
      return throwError(error.message);
    }

    let applicationError = error.headers.get('Application-Error');

    if (applicationError) {
      return throwError(applicationError);
    }

    let modelStateErrors: string = '';
    console.log(error);
    let serverError = error.error;

    if (!serverError.type) {
      for (let key in serverError) {
        if (serverError.hasOwnProperty(key))
          modelStateErrors += serverError[key] + '\n';
      }
    }

    console.log(modelStateErrors);

    modelStateErrors = modelStateErrors === '' ? null : modelStateErrors;
    return throwError(modelStateErrors || 'Server error');
  }
}
