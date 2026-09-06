import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Product } from '../../../core/models/product.model';
import { ProductService } from '../../../core/services/product.service';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './order-form.html',
  styleUrl: './order-form.scss',
})
export class OrderForm implements OnInit {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private orderService = inject(OrderService);
  private router = inject(Router);

  products: Product[] = [];
  submitting = false;

  form = this.fb.group({
    productId: ['', Validators.required],
    quantity: [1, [Validators.required, Validators.min(1)]],
  });

  ngOnInit() {
    this.productService.getProducts().subscribe((products) => (this.products = products));
  }

  submit() {
    if (this.form.invalid) return;
    this.submitting = true;
    const { productId, quantity } = this.form.value;
    this.orderService.addOrder(productId!, quantity!).subscribe(() => {
      this.submitting = false;
      this.router.navigate(['/orders']);
    });
  }
}