import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { Order } from '../../../core/models/order.model';
import { OrderService } from '../../../core/services/order.service';
import { ProductService } from '../../../core/services/product.service';
import { CartService } from '../../../core/services/cart.service';
import { Product } from '../../../core/models/product.model';

interface DisplayItem {
  productId: string;
  name: string;
  image: string;
  quantity: number;
  unitPrice: number;
  total: number;
}

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
  private cartService = inject(CartService);

  order = signal<Order | null>(null);
  displayItems = signal<DisplayItem[]>([]);
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

      this.productService.getProducts().subscribe((products) => {
        const items = order.orderItems.map((oi) => {
          const product = products.find((p) => p.id === oi.productId);
          return {
            productId: oi.productId,
            name: product?.name ?? 'Unknown product',
            image: product?.image ?? '',
            quantity: oi.quantity,
            unitPrice: oi.unitPrice,
            total: oi.unitPrice * oi.quantity,
          };
        });
        this.displayItems.set(items);
        this.loading.set(false);
      });
    });
  }

  getOrderNumber(): string {
    const order = this.order();
    return order ? 'ORD-' + order.id.slice(0, 8).toUpperCase() : '';
  }

  getSubtotal(): number {
    return this.displayItems().reduce((sum, item) => sum + item.total, 0);
  }

  getVat(): number {
    return this.getSubtotal() * 0.15;
  }

  getTotal(): number {
    return this.getSubtotal() + this.getVat();
  }

  getStatusLabel(): string {
    switch (this.order()?.status) {
      case 'pending': return 'Pending';
      case 'confirmed': return 'Processing';
      case 'shipped': return 'Shipped';
      case 'cancelled': return 'Cancelled';
      default: return '';
    }
  }

  getStatusIndex(): number {
    switch (this.order()?.status) {
      case 'pending': return 0;
      case 'confirmed': return 1;
      case 'shipped': return 2;
      default: return -1;
    }
  }

  goBack(): void {
    this.router.navigate(['/orders']);
  }

  reorder(): void {
    const items = this.displayItems();
    items.forEach((item) => {
      this.cartService.addItem(
        { productId: item.productId, name: item.name, price: item.unitPrice, image: item.image },
        item.quantity
      );
    });
    this.router.navigate(['/cart']);
  }
}