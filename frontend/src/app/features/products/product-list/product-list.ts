import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { Product } from '../../../core/models/product.model';
import { ProductService } from '../../../core/services/product.service';
import { CartService } from '../../../core/services/cart.service';
import { SearchService } from '../../../core/services/search.service';

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
  private searchService = inject(SearchService);

  products = signal<Product[]>([]);

  loading = signal(true);

  selectedFilter = signal('All');

  quantities = signal<Record<string, number>>({});

  addedFeedback = signal<Record<string, boolean>>({});

  // Pagination

  currentPage = signal(1);

  productsPerPage = 8;

  filters = [
    'All',
    'Stationery',
    'Paper',
    'Printing',
    'Office',
    'Cleaning',
    'Pantry',
  ];

  filteredProducts = computed(() => {

    const filter = this.selectedFilter();

    const search = this.searchService.searchTerm()
      .toLowerCase()
      .trim();

    const products = this.products();

    let filtered = products;

    // Category filter
    if (filter !== 'All') {
      filtered = filtered.filter(
        (product) => product.category === filter
      );
    }

    // Search filter
    if (search) {
      filtered = filtered.filter((product) =>
        product.name.toLowerCase().includes(search)
      );
    }

    return filtered;
  });

  totalPages = computed(() => {

    return Math.max(
      1,
      Math.ceil(
        this.filteredProducts().length / this.productsPerPage
      )
    );

  });

  paginatedProducts = computed(() => {

    const products = this.filteredProducts();

    const startIndex =
      (this.currentPage() - 1) * this.productsPerPage;

    const endIndex =
      startIndex + this.productsPerPage;

    return products.slice(startIndex, endIndex);

  });

  pageNumbers = computed(() => {

    const pages: number[] = [];

    for (let i = 1; i <= this.totalPages(); i++) {
      pages.push(i);
    }

    return pages;

  });

  ngOnInit() {

    this.productService.getProducts().subscribe((products) => {

      this.products.set(products);

      const initialQuantities: Record<string, number> = {};

      products.forEach((product) => {
        initialQuantities[product.id] = 1;
      });

      this.quantities.set(initialQuantities);

      this.loading.set(false);

    });

  }

  selectFilter(filter: string) {

    this.selectedFilter.set(filter);

    // Go back to page 1 whenever the category changes
    this.currentPage.set(1);

  }

  goToPage(page: number) {

    if (page < 1 || page > this.totalPages()) {
      return;
    }

    this.currentPage.set(page);

    // Scroll back to the products when changing page
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });

  }

  previousPage() {

    this.goToPage(this.currentPage() - 1);

  }

  nextPage() {

    this.goToPage(this.currentPage() + 1);

  }

  getQuantity(productId: string) {

    return this.quantities()[productId] ?? 1;

  }

  incrementQuantity(productId: string) {

    this.quantities.update((quantities) => ({

      ...quantities,

      [productId]:
        (quantities[productId] ?? 1) + 1,

    }));

  }

  decrementQuantity(productId: string) {

    this.quantities.update((quantities) => ({

      ...quantities,

      [productId]:
        Math.max(
          1,
          (quantities[productId] ?? 1) - 1
        ),

    }));

  }

  addToOrder(productId: string) {

    const qty = this.getQuantity(productId);

    const product = this.products().find(
      (product) => product.id === productId
    );

    if (!product) {
      return;
    }

    this.cartService.addItem(
      {
        productId: product.id,
        name: product.name,
        price: product.price,
        image: product.image,
      },
      qty
    );

    this.addedFeedback.update((feedback) => ({

      ...feedback,

      [productId]: true,

    }));

    setTimeout(() => {

      this.addedFeedback.update((feedback) => ({

        ...feedback,

        [productId]: false,

      }));

    }, 1500);

  }

}