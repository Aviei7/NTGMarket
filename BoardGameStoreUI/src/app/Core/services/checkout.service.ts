import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import {
  CheckoutOrderModel,
  CheckoutSubmitResultModel,
  CheckoutValidationModel,
  CheckoutValidationResultModel
} from '../../models/CheckoutModel/checkout.model';
import { Observable } from 'rxjs';
import { CheckoutInfoModel } from '../../models/CheckoutModel/checkoutInfo.model';
import { buildApiUrl } from '../config/api.config';


@Injectable({ providedIn: 'root' })
export class CheckoutService {

  constructor(private http: HttpClient) {}

  ValidateOrder(orderData: CheckoutValidationModel): Observable<CheckoutValidationResultModel> {
    return this.http.post<CheckoutValidationResultModel>(buildApiUrl('/Checkout/validate'), orderData);
  }

  SubmitOrder(orderData: CheckoutOrderModel): Observable<CheckoutSubmitResultModel> {
    return this.http.post<CheckoutSubmitResultModel>(buildApiUrl('/Checkout/submit'), orderData);
  }

  GetCheckoutInfo(): Observable<CheckoutInfoModel> {
    return this.http.get<CheckoutInfoModel>(buildApiUrl('/Checkout/info'));
  }
 
}
