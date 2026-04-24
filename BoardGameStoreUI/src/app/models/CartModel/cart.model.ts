import { CartItemModel } from './cart-item.model';

export interface CartModel {
    totalQuantity: number;
    totalPrice: number;
    items: CartItemModel[];
}
