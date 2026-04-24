import { Injectable } from '@angular/core';
import { HttpClient,HttpParams  } from '@angular/common/http'; 
import { Observable } from 'rxjs';
import { Category } from '../../../models/FilterModel/category';
import { FilterList } from '../../../models/FilterModel/filter-list.model';
import { FilterValue } from '../../../models/FilterModel/filter-value.model';
import { PriceRange } from '../../../models/FilterModel/price-range.model';
import { QueryParam } from '../../../models/FilterModel/query-param.model';
import { buildApiUrl } from '../../config/api.config';

import { BehaviorSubject } from 'rxjs';

 
@Injectable({
  providedIn: 'root'
})
export class FilterService {

  private apiUrl = ''; 
  private _filters$ = new BehaviorSubject<FilterList[]>([]);
  filters$ = this._filters$.asObservable();

  updateFilters(filters: FilterList[]) {
    this._filters$.next(filters);
  }

  get currentFilters(): FilterList[] {
    return this._filters$.value;
  }

   get currentFiltersChecked(): FilterList[] {
    return this._filters$.value.filter(filter => 
      filter.paramList.some((param: QueryParam) => param.checked)
    );
  }

  constructor(private http: HttpClient) {}


  getFilterList() {
    
    this.apiUrl = buildApiUrl('/Catalog/filterlist');

    return this.http.get<FilterList[]>(this.apiUrl);
  }

  getPriceRange()
  {
    this.apiUrl = buildApiUrl('/Catalog/price-range');

    return this.http.get<PriceRange>(this.apiUrl);
  }

  postProductsWithFilter(value: FilterValue): Observable<FilterValue> {
  
    this.apiUrl = buildApiUrl('/Catalog/products/filtered');
  
  
  
    return this.http.post<FilterValue>(this.apiUrl, value);
  }


  getCategories(catId?: number): Observable<Category[]> {
    
    this.apiUrl = buildApiUrl('/Catalog/category');
    let params = {};
    if (catId !== undefined) {
      params = { id: catId };
    }

    return this.http.get<Category[]>(this.apiUrl, { params });
  }
}
