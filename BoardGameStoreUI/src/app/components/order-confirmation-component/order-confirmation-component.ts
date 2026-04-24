import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-order-confirmation-component',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './order-confirmation-component.html',
  styleUrl: './order-confirmation-component.css'
})
export class OrderConfirmationComponent implements OnInit {
  public orderId = '';
  public paymentStatus = '';
  public paymentMethod = '';
  public paymentStatusClass = 'status-pill--pending';

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    const queryParams = this.route.snapshot.queryParamMap;
    const routeOrderId = queryParams.get('orderId');
    const routePaymentStatus = queryParams.get('paymentStatus');
    const routePaymentMethod = queryParams.get('paymentMethod');

    this.orderId = this.normalizeOrderId(routeOrderId);
    this.paymentStatus = routePaymentStatus ?? 'Awaiting payment confirmation';
    this.paymentMethod = routePaymentMethod ?? 'Selected payment method';
    this.paymentStatusClass = this.resolveStatusClass(this.paymentStatus);
  }

  private normalizeOrderId(orderId: string | null): string {
    if (!orderId) {
      return `#${this.generateMockOrderId()}`;
    }

    return orderId.startsWith('NTG-') || orderId.startsWith('#')
      ? orderId.startsWith('#') ? orderId : `#${orderId}`
      : `#${orderId}`;
  }

  private resolveStatusClass(status: string): string {
    const normalizedStatus = status.toLowerCase();

    if (normalizedStatus.includes('paid') || normalizedStatus.includes('confirmed')) {
      return 'status-pill--success';
    }

    if (normalizedStatus.includes('failed') || normalizedStatus.includes('declined')) {
      return 'status-pill--danger';
    }

    return 'status-pill--pending';
  }

  private generateMockOrderId(): string {
    return `NTG-${new Date().getFullYear()}-${Math.floor(100000 + Math.random() * 900000)}`;
  }
}
