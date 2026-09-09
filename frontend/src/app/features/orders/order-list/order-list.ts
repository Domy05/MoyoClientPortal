import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { ProductService } from '../../../core/services/product.service';
import { Order } from '../../../core/models/order.model';
import { Product } from '../../../core/models/product.model';

type StatusFilter = 'all' | Order['status'];

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-list.html',
  styleUrl: './order-list.scss',
})
export class OrderList implements OnInit {
  private router = inject(Router);
  private orderService = inject(OrderService);
  private productService = inject(ProductService);

  orders = signal<Order[]>([]);
  products = signal<Product[]>([]);
  loading = signal(true);
  selectedStatus = signal<StatusFilter>('all');
  currentPage = signal(1);

  filteredOrders = computed(() => {
    const status = this.selectedStatus();
    const orders = this.orders();
    return status === 'all' ? orders : orders.filter((o) => o.status === status);
  });

  ngOnInit() {
    this.productService.getProducts().subscribe((products) => {
      this.products.set(products);
      this.orderService.getOrders().subscribe((orders) => {
        this.orders.set(orders);
        this.loading.set(false);
      });
    });
  }

  getOrderTotal(order: Order): number {
    return order.orderItems.reduce((sum, item) => sum + item.unitPrice * item.quantity, 0);
  }

  getOrderNumber(order: Order): string {
    return 'ORD-' + order.id.slice(0, 8).toUpperCase();
  }

  getStatusLabel(status: Order['status']): string {
    switch (status) {
      case 'pending': return 'Pending';
      case 'confirmed': return 'Processing';
      case 'shipped': return 'Shipped';
      case 'cancelled': return 'Cancelled';
    }
  }

  selectStatus(status: StatusFilter) {
    this.selectedStatus.set(status);
  }

  viewOrder(order: Order): void {
    this.router.navigate(['/orders', order.id]);
  }

  previousPage(): void {
    if (this.currentPage() > 1) this.currentPage.update((p) => p - 1);
  }

  nextPage(): void {
    this.currentPage.update((p) => p + 1);
  }
}