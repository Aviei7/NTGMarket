import { Routes } from '@angular/router';
import { ProductComponent } from './components/Product/product-component/product-component';
import { CatalogComponent } from './components/Product/catalog-component/catalog-component';
import { MainComponent } from './components/main-component/main-component';
import { ProductCardComponent } from './components/Product/product-card-component/product-card-component';
import { AuthComponent } from './components/Users/auth-component/auth-component';
import { CabinetComponent } from './components/Users/cabinet-component/cabinet-component';
import { RegisterComponent } from './components/Users/register-component/register-component';
import { authGuard } from './Core/guard/auth.guard';
import { CartComponent } from './components/cart-component/cart-component';
import { CheckoutFormComponent } from './components/checkout-form-component/checkout-form-component';
import { OrderConfirmationComponent } from './components/order-confirmation-component/order-confirmation-component';


export const routes: Routes = [
  { path: '', component: MainComponent, pathMatch: 'full' },

  { path: 'testProductCard', component: ProductCardComponent },
  { path: 'login', component: AuthComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'cabinet', component: CabinetComponent, canActivate: [authGuard]},
  { path: 'cart', component: CartComponent},
  { path: 'checkout', component: CheckoutFormComponent },
  { path: 'checkout/confirmation', component: OrderConfirmationComponent },

  { path: 'catalog', component: CatalogComponent },
  { path: 'catalog/:slug/:id', component: CatalogComponent },

  { path: 'product/:id', component: ProductComponent },
  { path: '', redirectTo: '/', pathMatch: 'full' }
];
