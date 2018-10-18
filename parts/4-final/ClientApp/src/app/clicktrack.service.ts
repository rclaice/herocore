import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { TrackedItem } from './trackeditem';
import { MessageService } from './message.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })
export class ClicktrackService {

  private trackUrl = 'api/clicktrack';  // URL to web api
  

  constructor(
    private http: HttpClient,
    private messageService: MessageService) { }


  /** POST: add a track info the server */
  track (trackedItem: TrackedItem): Observable<TrackedItem> {
    return this.http.post<TrackedItem>(this.trackUrl, trackedItem, httpOptions)
    .pipe(
      tap((trackedItem: TrackedItem) => this.log(`tracked item w/ id=${trackedItem.id}`)),
      catchError(this.handleError<TrackedItem>('trackitem'))
    );
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    this.messageService.add(`HeroService: ${message}`);
  }
}