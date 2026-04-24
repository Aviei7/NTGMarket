import { Injectable } from '@angular/core';
import { HttpClient,HttpParams  } from '@angular/common/http'; 
import { DiscountModel } from '../../../models/LoyaltyModel/discount.model';
import { Observable } from 'rxjs';
import { buildApiUrl } from '../../config/api.config';

@Injectable({ providedIn: 'root' })
export class DiscountService {
  constructor(private http: HttpClient) {}

    GetDiscountByPromoCode(promoCode: string): Observable<DiscountModel> {
        const params = new HttpParams().set('promoCode', promoCode);
        return this.http.get<DiscountModel>(buildApiUrl('/Loyalty/GetDiscountByPromoCode'), { params });
    }
}
