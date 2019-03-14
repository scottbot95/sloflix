import { Observable } from 'rxjs';

export abstract class BaseService {
  constructor() {}

  protected handleError(error: any) {
    let applicationError = error.headers.get('Application-Error');

    if (applicationError) {
      return Observable.throw(applicationError);
    }

    let modelStateErrors: string = '';
    let serverError = error.json();

    if (!serverError.type) {
      for (let key in serverError) {
        if (serverError.hasOwnProperty(key))
          modelStateErrors += serverError[key] + '\n';
      }
    }

    modelStateErrors = modelStateErrors === '' ? null : modelStateErrors;
    return Observable.throw(modelStateErrors || 'Server error');
  }
}
