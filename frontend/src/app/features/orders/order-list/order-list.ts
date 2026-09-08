import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

export type OrderStatus =
  | 'processing'
  | 'shipped'
  | 'delivered'
  | 'cancelled';

export interface OrderItem {
  id: string;
  name: string;
  quantity: number;
  price: number;
  total: number;
}

export interface OrderRow {
  id: string;
  orderNumber: string;
  date: string;
  total: number;
  status: OrderStatus;
  items: OrderItem[];
}

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-list.html',
  styleUrl: './order-list.scss',
})
export class OrderList {
  private router = inject(Router);

  selectedStatus: 'all' | OrderStatus = 'all';
  currentPage = 1;

  orders: OrderRow[] = [
    {
      id: '1',
      orderNumber: 'ORD-10034',
      date: '2025-08-20',
      total: 2450.0,
      status: 'processing',
      items: [
        {
          id: '1',
          name: 'A4 Copy Paper',
          quantity: 5,
          price: 89.99,
          total: 449.95,
        },
        {
          id: '2',
          name: 'Biro Pens (Box of 50)',
          quantity: 2,
          price: 120.0,
          total: 240.0,
        },
      ],
    },

    {
      id: '2',
      orderNumber: 'ORD-10023',
      date: '2025-08-16',
      total: 1280.0,
      status: 'shipped',
      items: [
        {
          id: '3',
          name: 'A4 Lever Arch File',
          quantity: 8,
          price: 45.0,
          total: 360.0,
        },
        {
          id: '4',
          name: 'Coffee (1kg)',
          quantity: 4,
          price: 160.0,
          total: 640.0,
        },
      ],
    },

    {
      id: '3',
      orderNumber: 'ORD-10022',
      date: '2025-08-15',
      total: 980.0,
      status: 'delivered',
      items: [
        {
          id: '5',
          name: 'A4 Copy Paper',
          quantity: 10,
          price: 89.99,
          total: 899.9,
        },
      ],
    },

    {
      id: '4',
      orderNumber: 'ORD-10021',
      date: '2025-08-10',
      total: 1120.0,
      status: 'delivered',
      items: [
        {
          id: '6',
          name: 'Biro Pens (Box of 50)',
          quantity: 4,
          price: 120.0,
          total: 480.0,
        },
        {
          id: '7',
          name: 'A4 Lever Arch File',
          quantity: 5,
          price: 45.0,
          total: 225.0,
        },
      ],
    },

    {
      id: '5',
      orderNumber: 'ORD-10020',
      date: '2025-08-06',
      total: 760.0,
      status: 'cancelled',
      items: [
        {
          id: '8',
          name: 'Coffee (1kg)',
          quantity: 4,
          price: 160.0,
          total: 640.0,
        },
      ],
    },
  ];

  get filteredOrders(): OrderRow[] {
    if (this.selectedStatus === 'all') {
      return this.orders;
    }

    return this.orders.filter(
      (order) => order.status === this.selectedStatus
    );
  }

  getStatusLabel(status: OrderStatus): string {
    switch (status) {
      case 'processing':
        return 'Processing';

      case 'shipped':
        return 'Shipped';

      case 'delivered':
        return 'Delivered';

      case 'cancelled':
        return 'Cancelled';
    }
  }

  viewOrder(order: OrderRow): void {
    this.router.navigate(['/orders', order.id]);
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    this.currentPage++;
  }
}