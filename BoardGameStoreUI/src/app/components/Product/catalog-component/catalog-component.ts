import { Component, OnInit, inject } from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';



/*Services*/
import { ProductService } from '../../../Core/services/catalog.services/product.service';
import { FilterService } from '../../../Core/services/catalog.services/filter.service';
import { PaginationService } from '../../../Core/services/catalog.services/pagination.service';
/*Services*/

/*Model*/
import { Product } from '../../../models/ProductModel/product';
import { ProductResponse } from '../../../models/ProductModel/product-response';
import { Category } from '../../../models/FilterModel/category';
import { FilterList } from '../../../models/FilterModel/filter-list.model';
import { FilterValue } from '../../../models/FilterModel/filter-value.model';
import { QueryParam } from '../../../models/FilterModel/query-param.model';
/*Model*/

import { ActivatedRoute, Router, RouterModule } from '@angular/router';
/*PrimeNG*/
import { CardModule } from 'primeng/card';
import { PaginatorModule } from 'primeng/paginator';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { SliderModule } from 'primeng/slider';
import { AutoCompleteModule } from 'primeng/autocomplete';
/*PrimeNG*/

import { ProductCardComponent } from '../product-card-component/product-card-component';

@Component({
  selector: 'app-catalog-component',
  standalone: true,
  imports: [ProductCardComponent,CommonModule, RouterModule, ButtonModule, CardModule, PaginatorModule, CheckboxModule, FormsModule, ScrollPanelModule, SliderModule, AutoCompleteModule],
  templateUrl: './catalog-component.html',
  styleUrl: './catalog-component.css',
  providers: [PaginationService]

})
export class CatalogComponent implements OnInit {
  totalProductCount: number = 0;
  pageSize: number = 12;
  first: number = 0;
  hovered: number | null = null;
  products: Product[] = [];

  filterList: FilterList[] = [];
  filterValue: FilterValue[] = [];

  categoriesList: Category[] = [];
  categoryId: number | null = null;

  priceRange: number[] = [];
  minPrice: number = 0
  maxPrice: number = 0

  // UI state
  selectedSort: any = null;
  view: 'grid' | 'list' = 'grid';
  sortOptions = [
    { name: 'по популярності', value: 'popular' },
    { name: 'спочатку дешевше', value: 'priceAsc' },
    { name: 'спочатку дорожче', value: 'priceDesc' },
    { name: 'за назвою', value: 'name' },
  ];

  private filterChangeTimeout: any;

  constructor(private aRoute: ActivatedRoute, private route: Router, private productService: ProductService, private filterService: FilterService, private paginationService: PaginationService) { }

  ngOnInit(): void {
    this.aRoute.paramMap.subscribe(params => {
      const id = params.get('id');
      this.categoryId = id ? +id : null;
      this.loadProducts();
    });
    this.loadCategory();
    this.loadFilterList();
  }

  onCategorySelect(cat: Category) {
    // navigate to URL with slug for readability
    this.route.navigate(['/catalog', cat.slug, cat.categoryID]);
  }

  loadProducts() {
    const filters = this.filterService.currentFiltersChecked;
    if (filters.length > 0) {
      this.loardProductsWithFilter();
      return;
    }
    this.loadProductWithoutFilter();
  }

  loadProductWithoutFilter() {
    this.paginationService.page$.subscribe((page: number) => {

      this.productService.getProductsToCatalog(page, this.categoryId ?? undefined, this.pageSize).subscribe((data: ProductResponse) => {
        this.products = data.items;
        this.totalProductCount = data.totalCount;
        this.pageSize = data.pageSize;
        this.minPrice = data.minPrice;
        this.maxPrice = data.maxPrice;
        this.priceRange = [this.minPrice, this.maxPrice];
        this.applySorting();
      });
    });

  }

  loardProductsWithFilter() {
    this.paginationService.page$.subscribe((page: number) => {
      const filters = this.filterService.currentFilters;
      const requestBody = this.buildRequest(filters, page, this.categoryId ?? undefined);
      this.productService.postProductsWithFilter(requestBody).subscribe((data: ProductResponse) => {
        this.products = data.items;
        this.totalProductCount = data.totalCount;
        this.pageSize = data.pageSize;
        this.applySorting();
      });
    });
  }


  onPageChange(event: any) {
    this.paginationService.setSpecifiedNextPage(event.page);
    this.pageSize = event.rows;
    this.first = event.first;
    this.loadProducts();
  }

  onSortChange(event: any) {
    this.selectedSort = event.target.value;
    this.applySorting();
  }

  applySorting() {
    if (!this.selectedSort || this.products.length === 0) return;

    const type = this.selectedSort;
    this.products.sort((a: Product, b: Product) => {
      switch (type) {
        case 'priceAsc':
          return a.price - b.price;
        case 'priceDesc':
          return b.price - a.price;
        case 'name':
          return a.name.localeCompare(b.name);
        default:
          return 0;
      }
    });
  }

  onEnter(id: number) {
    this.hovered = id;
  }

  loadFilterList() {
    this.filterService.getFilterList().subscribe((data: FilterList[]) => {
      this.filterList = data;
    });
  }

  loadCategory() {
    this.filterService.getCategories().subscribe((data: Category[]) => {
      this.categoriesList = data;
    });
    this.filterList.forEach(f => {
      f.paramList = f.paramList || [];
      f.paramList.forEach((op: QueryParam) => {
        if (op.checked === undefined) op.checked = false;
        if (op.range === undefined) op.range = [];
        if (op.value === undefined) op.value = '';
      });
    });
  }

  onLeave() {
    this.hovered = null;
  }

  onPriceRangeChange(event: any) { const [min, max] = event.values; }

  onFilterChange() {
    // Отменяем предыдущий таймер
    clearTimeout(this.filterChangeTimeout);

    console.log("Click");

    this.filterService.updateFilters(this.filterList);
    // Ждём 400 мс, чтобы не вызывать API при каждом клике сразу
    this.filterChangeTimeout = setTimeout(() => {

      this.loardProductsWithFilter();
    }, 400);
  }

  buildRequest(filters: FilterList[], page: number, categoryId?: number) {
    const result: any = {};

    const allFilters: any[] = [];

    for (const f of filters) {
      // чекбоксы
      // const selected = f.paramList.filter(p => p.checked).map(p => p.paramID);
      // if (selected.length)
      //   allFilters.push({ fieldName: f.fieldName, filterValue: selected });

      const selectedParams = f.paramList.filter((p: QueryParam) => p.checked);

      for (const param of selectedParams) {
        allFilters.push({
          fieldName: f.fieldName,
          filterValue: param.paramID,
          queryParam: param.queryParam
        });
      }

      // диапазоны
      const rangeParam = f.paramList.find((p: QueryParam) => p.range && p.range.length === 2);
      if (rangeParam)
        allFilters.push({ fieldName: f.fieldName, filterValue: rangeParam.range });

      // инпуты
      const inputParam = f.paramList.find((p: QueryParam) => p.value);
      if (inputParam)
        allFilters.push({ fieldName: f.fieldName, filterValue: inputParam.value });
    }

    result.allFilter = allFilters;
    result.page = page;
    result.pageSize = this.pageSize;
    if (categoryId) result.categoryId = categoryId;

    console.log('built request:', result);
    return result;
  }

}
