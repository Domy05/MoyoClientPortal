import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

interface CartItem {
  id: number;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrl: './cart.scss',
})
export class Cart {

  constructor(private router: Router) {}

  cartItems: CartItem[] = [
    {
      id: 1,
      name: 'A4 Copy Paper',
      price: 89.99,
      quantity: 5,
      image: 'assets/products/copy-paper.png',
    },

    {
      id: 2,
      name: 'Biro Pens (Box of 50)',
      price: 120.00,
      quantity: 2,
      image: 'assets/products/biro-pens.png',
    },

    {
      id: 3,
      name: 'A4 Lever Arch File',
      price: 45.00,
      quantity: 3,
      image: 'assets/products/lever-arch-file.png',
    },

    {
      id: 4,
      name: 'Coffee (1kg)',
      price: 160.00,
      quantity: 1,
      image: 'assets/products/coffee.png',
    },
  ];


  increaseQuantity(item: CartItem): void {
    item.quantity++;
  }


  decreaseQuantity(item: CartItem): void {
    if (item.quantity > 1) {
      item.quantity--;
    }
  }


  removeItem(item: CartItem): void {
    this.cartItems = this.cartItems.filter(
      (cartItem) => cartItem.id !== item.id
    );
  }


  getItemTotal(item: CartItem): number {
    return item.price * item.quantity;
  }


  getSubtotal(): number {
    return this.cartItems.reduce(
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
    console.log('Placing order:', this.cartItems);

    // Backend/order submission can be connected here later.
  }

}