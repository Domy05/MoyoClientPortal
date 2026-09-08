import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../../core/models/product.model';
import { ProductService } from '../../../core/services/product.service';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss',
})
export class ProductDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private productService = inject(ProductService);
  private orderService = inject(OrderService);

  product = signal<Product | null>(null);
  loading = signal(true);
  quantity = signal(1);
  activeImage = signal('');
  added = signal(false);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.router.navigate(['/products']);
      return;
    }

    this.productService.getProduct(id).subscribe((product) => {
      if (!product) {
        this.router.navigate(['/products']);
        return;
      }
      this.product.set(product);
      this.activeImage.set(product.image);
      this.loading.set(false);
    });
  }

  selectImage(image: string) {
    this.activeImage.set(image);
  }

  incrementQuantity() {
    this.quantity.update((q) => q + 1);
  }

  decrementQuantity() {
    this.quantity.update((q) => Math.max(1, q - 1));
  }

  addToOrder() {
    const product = this.product();
    if (!product) return;
    this.orderService.addOrder(product.id, this.quantity()).subscribe(() => {
      this.added.set(true);
      setTimeout(() => this.added.set(false), 1500);
    });
  }

  goBack() {
  this.router.navigate(['/products']);
}
}