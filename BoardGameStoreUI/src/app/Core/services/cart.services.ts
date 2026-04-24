import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { CartModel } from '../../models/CartModel/cart.model';
import { buildApiUrl } from '../config/api.config';

@Injectable({ providedIn: 'root' })
export class CartServices {
  private readonly cartQuantitySubject = new BehaviorSubject<number>(0);

  readonly cartQuantity$ = this.cartQuantitySubject.asObservable();

  constructor(private http: HttpClient) {}

  AddInCart(productId: number): Observable<CartModel> {
    return this.http.get<CartModel>(buildApiUrl(`/Cart/addInCart/${productId}`), {
      withCredentials: true
    }).pipe(
      tap((cart) => this.syncCartQuantity(cart))
    );
  }

  GetCart(): Observable<CartModel> {
    return this.http.get<CartModel>(buildApiUrl('/Cart/cart'), {
      withCredentials: true
    }).pipe(
      tap((cart) => this.syncCartQuantity(cart))
    );
  }

  SubCartItem(productId: number): Observable<CartModel> {
    return this.http.get<CartModel>(buildApiUrl(`/Cart/subItem/${productId}`), {
      withCredentials: true
    }).pipe(
      tap((cart) => this.syncCartQuantity(cart))
    );
  }
  RemoveCartItem(productId: number): Observable<CartModel> {
     return this.http.get<CartModel>(buildApiUrl(`/Cart/removeItem/${productId}`), {
      withCredentials: true
    }).pipe(
      tap((cart) => this.syncCartQuantity(cart))
    );
  }


  AddCartItem(productId: number): Observable<CartModel> {
    return this.http.get<CartModel>(buildApiUrl(`/Cart/addItem/${productId}`), {
      withCredentials: true
    }).pipe(
      tap((cart) => this.syncCartQuantity(cart))
    );
  }

  ClearCart(): Observable<CartModel> {
    return this.http.get<CartModel>(buildApiUrl('/Cart/clearCart'), {
      withCredentials: true
    }).pipe(
      tap((cart) => this.syncCartQuantity(cart))
    );
  }

  private syncCartQuantity(cart: CartModel | null | undefined): void {
    this.cartQuantitySubject.next(cart?.totalQuantity ?? 0);
  }

}
