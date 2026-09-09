import { Injectable, signal, computed } from '@angular/core';

export interface CartItem {
  productId: string;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  items = signal<CartItem[]>([]);

  itemCount = computed(() => this.items().reduce((sum, i) => sum + i.quantity, 0));

  addItem(item: { productId: string; name: string; price: number; image: string }, quantity: number) {
    this.items.update((items) => {
      const existing = items.find((i) => i.productId === item.productId);
      if (existing) {
        return items.map((i) =>
          i.productId === item.productId ? { ...i, quantity: i.quantity + quantity } : i
        );
      }
      return [...items, { ...item, quantity }];
    });
  }

  increaseQuantity(productId: string) {
    this.items.update((items) =>
      items.map((i) => (i.productId === productId ? { ...i, quantity: i.quantity + 1 } : i))
    );
  }

  decreaseQuantity(productId: string) {
    this.items.update((items) =>
      items.map((i) =>
        i.productId === productId ? { ...i, quantity: Math.max(1, i.quantity - 1) } : i
      )
    );
  }

  removeItem(productId: string) {
    this.items.update((items) => items.filter((i) => i.productId !== productId));
  }

  clear() {
    this.items.set([]);
  }
}