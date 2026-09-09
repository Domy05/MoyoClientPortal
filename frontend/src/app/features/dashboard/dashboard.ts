import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrderService } from '../../core/services/order.service';
import { Order } from '../../core/models/order.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  private orderService = inject(OrderService);

  recentOrders = signal<Order[]>([]);
  totalOrders = signal(0);
  pendingOrders = signal(0);

  ngOnInit() {
    this.orderService.getOrders().subscribe((orders) => {
      this.recentOrders.set(orders.slice(0, 3));
      this.totalOrders.set(orders.length);
      this.pendingOrders.set(orders.filter((o) => o.status === 'pending').length);
    });
  }

  getOrderTotal(order: Order): number {
    return order.orderItems.reduce((sum, item) => sum + item.unitPrice * item.quantity, 0);
  }
}