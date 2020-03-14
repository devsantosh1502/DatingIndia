import { Injectable } from "@angular/core";
import {
  HttpInterceptor,
  HttpEvent,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse,
  HTTP_INTERCEPTORS
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorIntercept implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(err => {
        if (err instanceof HttpErrorResponse) {
            if(err.status === 401){
             return throwError(err.statusText);
            }
          const applicationError = err.headers.get("Application-Error");
          if (applicationError) {
            console.log(applicationError);
            return throwError(applicationError);
          }
          const serverError = err.error.errors;
          debugger;
          let modelStateError = "";
          if (serverError && typeof serverError === "object") {
            for (const key in serverError) {
              if (serverError[key]) {
                modelStateError += serverError[key] + "\n";
              }
            }
          }
          return throwError(modelStateError || serverError || "Server Error");
        }
      })
    );
  }
}
export const ErrorInterceptProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorIntercept,
  multi: true
};
