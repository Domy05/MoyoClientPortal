import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Product } from '../../../core/models/product.model';
import { ProductService } from '../../../core/services/product.service';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss',
})
export class ProductList implements OnInit {
  private productService = inject(ProductService);
  private cartService = inject(CartService);

  products = signal<Product[]>([]);
  loading = signal(true);
  selectedFilter = signal('All');
  quantities = signal<Record<string, number>>({});
  addedFeedback = signal<Record<string, boolean>>({});

  filters = ['All', 'Stationery', 'Paper', 'Printing', 'Office', 'Cleaning', 'Pantry'];

  filteredProducts = computed(() => {
    const filter = this.selectedFilter();
    const products = this.products();
    return filter === 'All' ? products : products.filter((p) => p.category === filter);
  });

  ngOnInit() {
    this.productService.getProducts().subscribe((products) => {
      this.products.set(products);
      const initialQuantities: Record<string, number> = {};
      products.forEach((p) => (initialQuantities[p.id] = 1));
      this.quantities.set(initialQuantities);
      this.loading.set(false);
    });
  }

  selectFilter(filter: string) {
    this.selectedFilter.set(filter);
  }

  getQuantity(productId: string) {
    return this.quantities()[productId] ?? 1;
  }

  incrementQuantity(productId: string) {
    this.quantities.update((q) => ({ ...q, [productId]: (q[productId] ?? 1) + 1 }));
  }

  decrementQuantity(productId: string) {
    this.quantities.update((q) => ({ ...q, [productId]: Math.max(1, (q[productId] ?? 1) - 1) }));
  }

  addToOrder(productId: string) {
    const qty = this.getQuantity(productId);
    const product = this.products().find((p) => p.id === productId);
    if (!product) return;

    this.cartService.addItem(
      { productId: product.id, name: product.name, price: product.price, image: product.image },
      qty
    );

    this.addedFeedback.update((f) => ({ ...f, [productId]: true }));
    setTimeout(() => {
      this.addedFeedback.update((f) => ({ ...f, [productId]: false }));
    }, 1500);
  }
}