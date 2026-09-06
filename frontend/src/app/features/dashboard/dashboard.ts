import { Component, OnInit, inject } from '@angular/core';
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

  recentOrders: Order[] = [];
  totalOrders = 0;
  pendingOrders = 0;

  ngOnInit() {
    this.orderService.getOrders().subscribe((orders) => {
      this.recentOrders = orders.slice(0, 3);
      this.totalOrders = orders.length;
      this.pendingOrders = orders.filter((o) => o.status === 'pending').length;
    });
  }
}