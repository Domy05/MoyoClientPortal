import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { ProductService } from '../../../core/services/product.service';
import { Order } from '../../../core/models/order.model';
import { Product } from '../../../core/models/product.model';

type StatusFilter = 'all' | Order['status'];

const ORDERS_PER_PAGE = 5;

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

    if (status === 'all') {
      return orders;
    }

    return orders.filter((order) => order.status === status);
  });

  totalPages = computed(() => {
    return Math.max(
      1,
      Math.ceil(this.filteredOrders().length / ORDERS_PER_PAGE)
    );
  });

  paginatedOrders = computed(() => {
    const orders = this.filteredOrders();
    const page = this.currentPage();

    const startIndex = (page - 1) * ORDERS_PER_PAGE;
    const endIndex = startIndex + ORDERS_PER_PAGE;

    return orders.slice(startIndex, endIndex);
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
    return order.orderItems.reduce(
      (sum, item) => sum + item.unitPrice * item.quantity,
      0
    );
  }

  getOrderNumber(order: Order): string {
    return 'ORD-' + order.id.slice(0, 8).toUpperCase();
  }

  getStatusLabel(status: Order['status']): string {
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
        return status;
    }
  }

  selectStatus(status: StatusFilter) {
    this.selectedStatus.set(status);

    // Always go back to page 1 when changing filters.
    this.currentPage.set(1);
  }

  viewOrder(order: Order): void {
    this.router.navigate(['/orders', order.id]);
  }

  previousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update((page) => page - 1);
    }
  }

  nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((page) => page + 1);
    }
  }
}