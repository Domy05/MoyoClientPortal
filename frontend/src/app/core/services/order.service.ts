import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order } from '../models/order.model';

const API_URL = 'http://localhost:5141/api/orders';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);

  getOrders() {
    return this.http.get<Order[]>(API_URL);
  }

  getOrder(id: string) {
    return this.http.get<Order>(`${API_URL}/${id}`);
  }

  addOrder(items: { productId: string; quantity: number; unitPrice: number }[]) {
    return this.http.post<Order>(API_URL, { items });
  }
}