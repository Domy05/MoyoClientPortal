import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';

export interface CartItem {
  productId: string;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);

  private apiUrl = 'http://localhost:5141/api/cart';

  items = signal<CartItem[]>([]);

  loading = signal(false);

  itemCount = computed(() =>
    this.items().reduce((sum, item) => sum + item.quantity, 0)
  );

  constructor() {
    this.loadCart();
  }

  private getClientId(): string | null {
    const client = this.authService.getClient();

    if (!client) {
      return null;
    }

    return client.clientId;
  }

  loadCart() {
    const clientId = this.getClientId();

    if (!clientId) {
      this.items.set([]);
      return;
    }

    this.loading.set(true);

    this.http
      .get<CartItem[]>(`${this.apiUrl}/${clientId}`)
      .subscribe({
        next: (items) => {
          this.items.set(items);
          this.loading.set(false);
        },
        error: (error) => {
          console.error('Failed to load cart:', error);
          this.items.set([]);
          this.loading.set(false);
        }
      });
  }

  addItem(
    item: {
      productId: string;
      name: string;
      price: number;
      image: string;
    },
    quantity: number
  ) {
    const clientId = this.getClientId();

    if (!clientId) {
      console.error('Cannot add to cart: user is not logged in.');
      return;
    }

    this.http
      .post(`${this.apiUrl}/${clientId}`, {
        productId: item.productId,
        quantity: quantity
      })
      .subscribe({
        next: () => {
          this.loadCart();
        },
        error: (error) => {
          console.error('Failed to add item to cart:', error);
        }
      });
  }

  increaseQuantity(productId: string) {
    const item = this.items().find(
      (i) => i.productId === productId
    );

    if (!item) {
      return;
    }

    const newQuantity = item.quantity + 1;

    this.updateQuantity(productId, newQuantity);
  }

  decreaseQuantity(productId: string) {
    const item = this.items().find(
      (i) => i.productId === productId
    );

    if (!item) {
      return;
    }

    const newQuantity = Math.max(1, item.quantity - 1);

    this.updateQuantity(productId, newQuantity);
  }

  private updateQuantity(
    productId: string,
    quantity: number
  ) {
    const clientId = this.getClientId();

    if (!clientId) {
      return;
    }

    this.http
      .put(
        `${this.apiUrl}/${clientId}/${productId}`,
        {
          quantity: quantity
        }
      )
      .subscribe({
        next: () => {
          this.loadCart();
        },
        error: (error) => {
          console.error(
            'Failed to update cart quantity:',
            error
          );
        }
      });
  }

  removeItem(productId: string) {
    const clientId = this.getClientId();

    if (!clientId) {
      return;
    }

    this.http
      .delete(
        `${this.apiUrl}/${clientId}/${productId}`
      )
      .subscribe({
        next: () => {
          this.loadCart();
        },
        error: (error) => {
          console.error(
            'Failed to remove cart item:',
            error
          );
        }
      });
  }

  clear() {
    const clientId = this.getClientId();

    if (!clientId) {
      this.items.set([]);
      return;
    }

    this.http
      .delete(`${this.apiUrl}/${clientId}`)
      .subscribe({
        next: () => {
          this.items.set([]);
        },
        error: (error) => {
          console.error(
            'Failed to clear cart:',
            error
          );
        }
      });
  }
}