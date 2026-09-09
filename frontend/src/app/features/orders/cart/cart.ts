import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { CartService } from '../../../core/services/cart.service';
import { OrderService } from '../../../core/services/order.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrl: './cart.scss',
})
export class Cart implements OnInit {

  cartService = inject(CartService);

  private orderService = inject(OrderService);
  private authService = inject(AuthService);
  private router = inject(Router);

  placingOrder = false;
  orderError = '';

  ngOnInit(): void {
    this.cartService.loadCart();
  }

  increaseQuantity(productId: string): void {
    this.cartService.increaseQuantity(productId);
  }

  decreaseQuantity(productId: string): void {
    this.cartService.decreaseQuantity(productId);
  }

  removeItem(productId: string): void {
    this.cartService.removeItem(productId);
  }

  getItemTotal(
    item: { price: number; quantity: number }
  ): number {
    return item.price * item.quantity;
  }

  getSubtotal(): number {
    return this.cartService.items().reduce(
      (total, item) => total + this.getItemTotal(item),
      0
    );
  }

  getVat(): number {
    return this.getSubtotal() * 0.15;
  }

  getGrandTotal(): number {
    return this.getSubtotal() + this.getVat();
  }

  goBack(): void {
    this.router.navigate(['/products']);
  }

  placeOrder(): void {

    const items = this.cartService.items();

    if (items.length === 0 || this.placingOrder) {
      return;
    }

    const client = this.authService.getClient();

    if (!client) {
      this.orderError = 'You must be logged in to place an order.';
      return;
    }

    const clientId = client.clientId;

    if (!clientId) {
      this.orderError = 'Unable to identify your account.';
      return;
    }

    this.placingOrder = true;
    this.orderError = '';

    const orderItems = items.map((item) => ({
      productId: item.productId,
      quantity: item.quantity,
    }));

    this.orderService
      .addOrder(clientId, orderItems)
      .subscribe({

        next: (order) => {

          this.cartService.clear();

          this.router.navigate([
            '/orders',
            order.id
          ]);
        },

        error: (error) => {

          console.error(
            'Failed to place order:',
            error
          );

          if (typeof error.error === 'string') {

            this.orderError = error.error;

          } else if (
            error.error?.message
          ) {

            this.orderError =
              error.error.message;

          } else {

            this.orderError =
              'Failed to place order. Please try again.';
          }

          this.placingOrder = false;
        },
      });
  }
}