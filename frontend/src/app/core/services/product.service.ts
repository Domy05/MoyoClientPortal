import { Injectable } from '@angular/core';
import { of, delay } from 'rxjs';
import { Product } from '../models/product.model';

const MOCK_PRODUCTS: Product[] = [
  { id: '1', name: 'Widget A', price: 99.99, stock: 42 },
  { id: '2', name: 'Widget B', price: 149.5, stock: 10 },
];

@Injectable({ providedIn: 'root' })
export class ProductService {
  getProducts() {
    return of(MOCK_PRODUCTS).pipe(delay(300));
  }
}