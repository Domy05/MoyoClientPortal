import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { Order } from '../../../core/models/order.model';
import { OrderService } from '../../../core/services/order.service';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-details.html',
  styleUrl: './order-details.scss',
})
export class OrderDetails implements OnInit {

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private orderService = inject(OrderService);
  private productService = inject(ProductService);

  order = signal<Order | null>(null);
  product = signal<Product | null>(null);

  loading = signal(true);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.router.navigate(['/orders']);
      return;
    }

    this.orderService.getOrder(id).subscribe((order) => {

      if (!order) {
        this.router.navigate(['/orders']);
        return;
      }

      this.order.set(order);

      this.productService.getProduct(order.productId).subscribe((product) => {

        this.product.set(product ?? null);

        this.loading.set(false);
      });
    });
  }


  getSubtotal(): number {
    const order = this.order();
    const product = this.product();

    if (!order || !product) {
      return 0;
    }

    return product.price * order.quantity;
  }


  getVat(): number {
    return this.getSubtotal() * 0.15;
  }


  getTotal(): number {
    return this.getSubtotal() + this.getVat();
  }


  getStatusLabel(): string {
    const status = this.order()?.status;

    switch (status) {
      case 'pending':
        return 'Pending';

      case 'confirmed':
        return 'Processing';

      case 'shipped':
        return 'Shipped';

      case 'cancelled':
        return 'Cancelled';

      default:
        return '';
    }
  }


  getStatusIndex(): number {
    const status = this.order()?.status;

    switch (status) {
      case 'pending':
        return 0;

      case 'confirmed':
        return 1;

      case 'shipped':
        return 2;

      default:
        return -1;
    }
  }


  goBack(): void {
    this.router.navigate(['/orders']);
  }


  reorder(): void {
    const order = this.order();

    if (!order) {
      return;
    }

    console.log('Reordering:', order);
  }
}