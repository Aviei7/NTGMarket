import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CartServices } from '../../Core/services/cart.services';
import { CartModel } from '../../models/CartModel/cart.model';
import { RouterLink} from '@angular/router';
import { DiscountService } from '../../Core/services/loyalty.services/discount.service';
import { DiscountModel } from '../../models/LoyaltyModel/discount.model';
import { CheckoutFormComponent } from '../checkout-form-component/checkout-form-component';
import { CartItemModel } from '../../models/CartModel/cart-item.model';
import {
  buildProductImageUrl,
  swapToProductPlaceholder
} from '../../Core/utils/product-image.util';

@Component({
  selector: 'app-cart-component',
  standalone: true,
  imports: [RouterLink, CommonModule, CheckoutFormComponent],
  templateUrl: './cart-component.html',
  styleUrl: './cart-component.css'
})
export class CartComponent {

  CartInfo: CartModel = {} as CartModel;
  discountPercentage: number = 0;
  promoErrorMessage: string = '';
  appliedPromoCode: string = '';


  constructor(
    private cartServices: CartServices,
    private discountService: DiscountService) {}

  ngOnInit(): void {
    this.cartServices.GetCart().subscribe({
      next: (CartResponse: CartModel) => {
        this.CartInfo = CartResponse;
      }
    });
  }

  OnRemoveItem(productId: number): void {
    this.cartServices.RemoveCartItem(productId).subscribe({
      next: (CartResponse: CartModel) => {
        this.CartInfo = CartResponse;
      },
      error: (error) => console.error('Failed to remove item from cart', error)
    });
  }

  OnSubItem(productId: number): void {
    this.cartServices.SubCartItem(productId).subscribe({
      next: (CartResponse: CartModel) => {
        this.CartInfo = CartResponse;
      },
      error: (error) => console.error('Failed to subtract item from cart', error)
    });
  }

    OnClearCart(): void {
      this.cartServices.ClearCart().subscribe({
        next: (CartResponse: CartModel) => {
          this.CartInfo = CartResponse;
        },
        error: (error) => console.error('Failed to clear cart', error)
      });
    }

      OnAddItem(productId: number): void {
        this.cartServices.AddCartItem(productId).subscribe({
          next: (CartResponse: CartModel) => {
            this.CartInfo = CartResponse;
          },
          error: (error) => console.error('Failed to add item to cart', error)
        });
      }

    applyPromoCode(promoCode: string): void {
      const normalizedPromoCode = promoCode.trim();

      if (!normalizedPromoCode) {
        this.promoErrorMessage = 'Enter promo code';
        this.discountPercentage = 0;
        this.appliedPromoCode = '';
        return;
      }

      this.discountService.GetDiscountByPromoCode(normalizedPromoCode).subscribe({
        next: (discount: DiscountModel) => {
          this.discountPercentage = Number(discount.discountPercentage);
          this.appliedPromoCode = discount.promoCode;
          this.promoErrorMessage = '';
        },
        error: (error) => {
          this.discountPercentage = 0;
          this.appliedPromoCode = '';

          if (error.status === 404) {
            this.promoErrorMessage = 'Promo code does not exist';
            return;
          }

          this.promoErrorMessage = 'Failed to apply promo code';
        }
      });
    }


  getItemImageUrl(item: CartItemModel): string {
    return buildProductImageUrl(item.imageUrl, null, item.productName);
  }

  onItemImageError(event: Event, item: CartItemModel): void {
    swapToProductPlaceholder(event, null, item.productName);
  }

  get itemsCount(): number {
    return this.CartInfo?.items.reduce((total, item) => total + item.quantity, 0) || 0;
  }

  get subtotal(): number {
    return this.CartInfo?.items.reduce((total, item) => total + item.unitPrice * item.quantity, 0) || 0;
  }

  get calculateDiscount(): number {
    if (this.discountPercentage === 0) {
      return 0;
    }
    return this.subtotal * (this.discountPercentage / 100);
  }

  get delivery(): number {
    return this.subtotal > 3000 ? 0 : 150;
  }

  get totalSum(): number {
    return this.subtotal - this.calculateDiscount + this.delivery;
  }
}
