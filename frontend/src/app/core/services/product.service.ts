import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product.model';

const API_URL = 'http://localhost:5141/api/products';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private http = inject(HttpClient);

  getProducts() {
    return this.http.get<Product[]>(API_URL);
  }

  getProduct(id: string) {
    return this.http.get<Product>(`${API_URL}/${id}`);
  }
}