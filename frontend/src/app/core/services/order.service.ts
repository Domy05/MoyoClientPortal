import { Injectable } from '@angular/core';
import { of, delay } from 'rxjs';
import { Order } from '../models/order.model';

const MOCK_ORDERS: Order[] = [
  { id: '1', productId: '1', quantity: 2, status: 'pending', createdAt: new Date().toISOString() },
  { id: '2', productId: '2', quantity: 1, status: 'confirmed', createdAt: new Date().toISOString() },
];

@Injectable({ providedIn: 'root' })
export class OrderService {
  private orders: Order[] = [...MOCK_ORDERS];

  getOrders() {
    return of(this.orders).pipe(delay(300));
  }

  getOrder(id: string) {
    return of(this.orders.find((o) => o.id === id)).pipe(delay(200));
  }

  addOrder(productId: string, quantity: number) {
    const newOrder: Order = {
      id: crypto.randomUUID(),
      productId,
      quantity,
      status: 'pending',
      createdAt: new Date().toISOString(),
    };
    this.orders = [...this.orders, newOrder];
    return of(newOrder).pipe(delay(300));
  }
}