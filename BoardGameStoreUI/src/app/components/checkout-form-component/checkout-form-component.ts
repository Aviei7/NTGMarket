import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CheckoutService } from '../../Core/services/checkout.service';
import { DeliveryMethod } from '../../models/CheckoutModel/deliveryMethods';
import { PaymentMethod } from '../../models/CheckoutModel/paymentMethods';
import { CheckoutOrderModel } from '../../models/CheckoutModel/checkout.model';
import { FormsModule } from '@angular/forms';
import { finalize, switchMap } from 'rxjs';

@Component({
  selector: 'app-checkout-form-component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './checkout-form-component.html',
  styleUrl: './checkout-form-component.css'
})
export class CheckoutFormComponent implements OnInit {
  public deliveryMethods: DeliveryMethod[] = [];

  public paymentMethods: PaymentMethod[] = [];

  public validationErrors: string[] = [];
  public isSubmitting = false;

  order: CheckoutOrderModel = {
    City: '',
    Address: '',
    Region: '',
    PostalCode: '',
    DeliveryMethod: 0,
    PaymentMethod: 0,
    PaymentStatusId: 1,
    TotalPrice: 0,
    FirstName: '',
    LastName: '',
    Email: '',
    Phone: '',
    Comment: ''
  };

  constructor(
    private checkoutService: CheckoutService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.GetCheckoutInfo();
  }

  ConfirmOrder() {
    this.validationErrors = [];
    this.isSubmitting = true;

    this.checkoutService
      .ValidateOrder({
        Email: this.order.Email,
        Phone: this.order.Phone
      })
      .pipe(
        switchMap(validationResult => {
          if (!validationResult.isValid) {
            this.validationErrors = validationResult.errors;
            throw new Error('checkout-validation-failed');
          }

          return this.checkoutService.SubmitOrder(this.order);
        }),
        finalize(() => {
          this.isSubmitting = false;
        })
      )
      .subscribe({
        next: result => {
          const selectedPaymentMethod = this.getSelectedPaymentMethodName();

          void this.router.navigate(['/checkout/confirmation'], {
            queryParams: {
              orderId: result.orderId || this.generateMockOrderId(),
              paymentStatus: this.buildMockPaymentStatus(selectedPaymentMethod),
              paymentMethod: selectedPaymentMethod
            }
          });
        },
        error: error => {
          if (error?.message === 'checkout-validation-failed') {
            return;
          }

          this.validationErrors = ['Unable to submit order. Please try again.'];
        }
      });
  }

  GetCheckoutInfo() {
    this.checkoutService.GetCheckoutInfo().subscribe(info => {
      this.deliveryMethods = info.deliveryMethods;
      this.paymentMethods = info.paymentMethods;

      this.order.DeliveryMethod = this.deliveryMethods[0]?.deliveryId ?? 0;
      this.order.PaymentMethod = this.paymentMethods[0]?.paymentId ?? 0;
    });
  }

  private getSelectedPaymentMethodName(): string {
    return this.paymentMethods.find(method => method.paymentId === this.order.PaymentMethod)?.paymentName
      ?? 'Selected payment method';
  }

  private buildMockPaymentStatus(paymentMethodName: string): string {
    const normalizedName = paymentMethodName.toLowerCase();

    if (normalizedName.includes('cash') || normalizedName.includes('delivery')) {
      return 'Payment on delivery';
    }

    if (normalizedName.includes('card') || normalizedName.includes('online')) {
      return 'Awaiting payment confirmation';
    }

    return 'Pending manual confirmation';
  }

  private generateMockOrderId(): string {
    return `NTG-${new Date().getFullYear()}-${Math.floor(100000 + Math.random() * 900000)}`;
  }
}
