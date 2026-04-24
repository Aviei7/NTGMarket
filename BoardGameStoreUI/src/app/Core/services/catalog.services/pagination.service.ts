import { Injectable } from '@angular/core';
import { BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaginationService {
   public page$ = new BehaviorSubject<number>(1);

   getValue(){
    return this.page$.asObservable(); 
   }

  setAutoNextPage(){
    this.page$.next(this.page$.getValue() + 1);
  }

  setSpecifiedNextPage(page: number) {
    this.page$.next(page + 1);
  }
  setOnFirst(page: number) {
    this.page$.next(1);
  }

}
