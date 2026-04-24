import { Injectable } from '@angular/core';
import { HttpClient,HttpParams  } from '@angular/common/http'; 
import { Observable } from 'rxjs';
import { buildApiUrl } from '../../config/api.config';

/*Model*/
import { Product } from '../../../models/ProductModel/product';
import { ProductResponse } from '../../../models/ProductModel/product-response';
/*Model*/
@Injectable({
  providedIn: 'root'
})
export class ProductService {

    private apiUrl: string = '';
    Page: number = 1;
      totalcount: number = 0;
      pageSize: number = 12;
      first: number = 0;
      hovered: number | null = null;
     products: Product[] = [];

  constructor(private http: HttpClient) {}


  getProductsToCatalog(page: number, categoryId?: number, pageSize: number = this.pageSize): Observable<ProductResponse> {

    this.apiUrl = buildApiUrl('/Catalog/products');

    let params = new HttpParams()
      .set('Page', page)
      .set('PageSize', pageSize.toString());

    if (categoryId !== undefined) {
      params = params.set('categoryId', categoryId.toString());
    }

    return this.http.get<ProductResponse>(this.apiUrl, { params });
  }

   postProductsWithFilter(requestBody: any): Observable<ProductResponse> {

    this.apiUrl = buildApiUrl('/Catalog/products/filtered');

    console.log('Sending JSON:', JSON.stringify(requestBody, null, 2));

    return this.http.post<ProductResponse>(this.apiUrl, requestBody );
  }

  getProductsToInfinityScroll(page: number, pageSize: number): Observable<ProductResponse> {
    this.apiUrl = buildApiUrl('/Catalog/products');

    let params = new HttpParams().set('Page', page.toString()).set('pageSize', pageSize.toString())

    return this.http.get<ProductResponse>(this.apiUrl, { params });
  }
}
