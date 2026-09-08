import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';

import {
  Order,
  OrderStatus
} from '../models/order.model';


const MOCK_ORDERS: Order[] = [

  {
    id: '1',
    orderNumber: 'ORD-10034',
    productId: '1',
    quantity: 5,
    total: 2450.00,
    status: 'pending',
    createdAt: '2025-08-20T00:00:00.000Z',
  },

  {
    id: '2',
    orderNumber: 'ORD-10023',
    productId: '2',
    quantity: 2,
    total: 1280.00,
    status: 'shipped',
    createdAt: '2025-08-16T00:00:00.000Z',
  },

  {
    id: '3',
    orderNumber: 'ORD-10022',
    productId: '3',
    quantity: 3,
    total: 980.00,
    status: 'delivered',
    createdAt: '2025-08-15T00:00:00.000Z',
  },

  {
    id: '4',
    orderNumber: 'ORD-10021',
    productId: '4',
    quantity: 4,
    total: 1120.00,
    status: 'delivered',
    createdAt: '2025-08-10T00:00:00.000Z',
  },

  {
    id: '5',
    orderNumber: 'ORD-10020',
    productId: '5',
    quantity: 2,
    total: 760.00,
    status: 'cancelled',
    createdAt: '2025-08-06T00:00:00.000Z',
  },

];


@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private orders: Order[] = [...MOCK_ORDERS];


  getOrders(): Observable<Order[]> {

    return of(this.orders).pipe(
      delay(300)
    );

  }


  getOrder(id: string): Observable<Order | undefined> {

    return of(
      this.orders.find(
        (order) => order.id === id
      )
    ).pipe(
      delay(200)
    );

  }


  addOrder(
    productId: string,
    quantity: number
  ): Observable<Order> {

    const newOrder: Order = {

      id: crypto.randomUUID(),

      orderNumber: `ORD-${10000 + this.orders.length + 1}`,

      productId,

      quantity,

      total: 0,

      status: 'pending',

      createdAt: new Date().toISOString(),

    };


    this.orders = [
      ...this.orders,
      newOrder
    ];


    return of(newOrder).pipe(
      delay(300)
    );

  }

}