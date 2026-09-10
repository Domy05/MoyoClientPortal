import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Product } from '../../../core/models/product.model';
import { ProductList } from './product-list';

const PRODUCTS: Product[] = [
  {
    id: '1',
    name: 'Notebook',
    price: 5,
    stock: 10,
    category: 'Paper',
    image: 'notebook.jpg',
  },
  {
    id: '2',
    name: 'Desk Organizer',
    price: 12,
    stock: 4,
    category: 'Office',
    image: 'organizer.jpg',
  },
];

describe('ProductList', () => {
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductList],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should create', () => {
    const fixture = TestBed.createComponent(ProductList);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should load products on init', () => {
    const fixture = TestBed.createComponent(ProductList);
    fixture.detectChanges();

    const request = httpMock.expectOne('http://localhost:5141/api/products');
    expect(request.request.method).toBe('GET');
    request.flush(PRODUCTS);

    expect(fixture.componentInstance.products()).toEqual(PRODUCTS);
    expect(fixture.componentInstance.loading()).toBe(false);
  });

  it('should filter products by category when a pill is selected', () => {
    const fixture = TestBed.createComponent(ProductList);
    fixture.detectChanges();

    const request = httpMock.expectOne('http://localhost:5141/api/products');
    request.flush(PRODUCTS);

    fixture.componentInstance.selectFilter('Paper');

    expect(fixture.componentInstance.filteredProducts()).toEqual([PRODUCTS[0]]);
  });
});
