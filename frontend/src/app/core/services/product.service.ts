import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product.model';
import { environment } from '../../../environments/environment';

const API_URL = `${environment.apiBaseUrl}/products`;

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
