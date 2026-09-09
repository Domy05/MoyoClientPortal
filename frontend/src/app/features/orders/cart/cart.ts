import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrl: './cart.scss',
})
export class Cart {
  cartService = inject(CartService);
  private orderService = inject(OrderService);
  private router = inject(Router);

  increaseQuantity(productId: string): void {
    this.cartService.increaseQuantity(productId);
  }

  decreaseQuantity(productId: string): void {
    this.cartService.decreaseQuantity(productId);
  }

  removeItem(productId: string): void {
    this.cartService.removeItem(productId);
  }

  getItemTotal(item: { price: number; quantity: number }): number {
    return item.price * item.quantity;
  }

  getSubtotal(): number {
    return this.cartService.items().reduce((total, item) => total + this.getItemTotal(item), 0);
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
    if (items.length === 0) return;

    const orderItems = items.map((item) => ({
      productId: item.productId,
      quantity: item.quantity,
      unitPrice: item.price,
    }));

    this.orderService.addOrder(orderItems).subscribe((order) => {
      this.cartService.clear();
      this.router.navigate(['/orders', order.id]);
    });
  }
}