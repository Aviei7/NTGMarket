import { Component, Input, OnDestroy } from '@angular/core';
import { Product } from '../../../models/ProductModel/product';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartServices } from '../../../Core/services/cart.services';
import {
  buildProductImageUrl,
  swapToProductPlaceholder
} from '../../../Core/utils/product-image.util';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './product-card-component.html',
  styleUrls: ['./product-card-component.css']
})
export class ProductCardComponent implements OnDestroy {
  @Input() product!: Product;

  isAdding = false;
  isAdded = false;
  private addedStateTimeoutId: ReturnType<typeof setTimeout> | null = null;

  constructor(private cartServices: CartServices) {}

  addToCart(): void {
    if (this.isAdding) {
      return;
    }

    this.isAdding = true;

    this.cartServices.AddInCart(this.product.id).subscribe({
      next: () => {
        this.showAddedState();
      },
      error: (error) => console.error('Failed to add product to cart', error),
      complete: () => {
        this.isAdding = false;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.addedStateTimeoutId) {
      clearTimeout(this.addedStateTimeoutId);
    }
  }

  get productImageUrl(): string {
    return buildProductImageUrl(
      this.product?.primaryImageUrl,
      this.product?.categoryName,
      this.product?.name
    );
  }

  onImageError(event: Event): void {
    swapToProductPlaceholder(event, this.product?.categoryName, this.product?.name);
  }

  private showAddedState(): void {
    this.isAdded = true;

    if (this.addedStateTimeoutId) {
      clearTimeout(this.addedStateTimeoutId);
    }

    this.addedStateTimeoutId = setTimeout(() => {
      this.isAdded = false;
      this.addedStateTimeoutId = null;
    }, 1400);
  }
}
