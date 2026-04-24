import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

/*Services*/
import { ProductService } from '../../Core/services/catalog.services/product.service';
import { FilterService } from '../../Core/services/catalog.services/filter.service';
import { PaginationService } from '../../Core/services/catalog.services/pagination.service';
import { DarkModeService } from '../../Core/services/global.services/dark-mode.service';
/*Services*/

/*Model*/
import { Product } from '../../models/ProductModel/product';
import { ProductResponse } from '../../models/ProductModel/product-response';
import { Category } from '../../models/FilterModel/category';
/*Model*/

/*Primeng*/
import { SplitterModule } from 'primeng/splitter';
import { ScrollerModule } from 'primeng/scroller';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
/*Primeng*/

import { ActivatedRoute, Router, RouterModule} from '@angular/router';
import { ProductCardComponent } from '../Product/product-card-component/product-card-component';

@Component({
  selector: 'app-main-component',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, SplitterModule, ScrollerModule, ProductCardComponent, ButtonModule, CardModule],
  templateUrl: './main-component.html',
  styleUrl: './main-component.css',
  providers: [PaginationService]
})
export class MainComponent {
  totalProductCount: number = 0;
  pageSize: number = 12;
  first: number = 0;
  hovered: number | null = null;
 products: Product[] = [];
 view: 'grid' | 'list' = 'grid';
  categoriesList: Category[] = [];
  categoryId: number | null = null;

constructor(private aRoute: ActivatedRoute, private route: Router, private productService: ProductService, private filter: FilterService, private paginationService: PaginationService){}

  ngOnInit(): void {
    this.aRoute.paramMap.subscribe(params => {
    const id = params.get('id');
    this.categoryId = id ? +id : null;
    this.loadProducts();
  });
    this.loadCategory();
  }

  loadProducts(){

    this.paginationService.page$.subscribe(page => {

    
      this.productService.getProductsToCatalog(page, this.categoryId ?? undefined).subscribe((data: ProductResponse) => {
      this.products = this.products = [...this.products, ...data.items];
      this.totalProductCount = data.totalCount;
      this.pageSize = data.pageSize;
    });
  });
  }

loadCategory(){
  this.filter.getCategories().subscribe((data: Category[]) => {
      this.categoriesList = data;
      console.log(this.categoriesList);
      
    });
}


loadMoreItem(){
  if (this.products.length >= this.totalProductCount) return;
    this.paginationService.setAutoNextPage();
    this.loadProducts();
}

categoryEnter(Category: any){
  this.route.navigate(['/catalog', Category.slug,Category.categoryID]);
  console.log('/catalog' + Category.slug + Category.categoryID);
}



  onEnter(id: number) {
  this.hovered = id;
}


onLeave() {
  this.hovered = null;
}
}

