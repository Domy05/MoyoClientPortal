import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { Product } from '../../../core/models/product.model';
import { ProductDetail } from './product-detail';

const PRODUCT: Product = {
  id: '1',
  name: 'Notebook',
  price: 5,
  stock: 10,
  category: 'Paper',
  image: 'notebook.jpg',
};

describe('ProductDetail', () => {
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductDetail],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { paramMap: convertToParamMap({ id: '1' }) },
          },
        },
      ],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should create', () => {
    const fixture = TestBed.createComponent(ProductDetail);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should load the product matching the route id', () => {
    const fixture = TestBed.createComponent(ProductDetail);
    fixture.detectChanges();

    const request = httpMock.expectOne('http://localhost:5141/api/products/1');
    expect(request.request.method).toBe('GET');
    request.flush(PRODUCT);

    expect(fixture.componentInstance.product()).toEqual(PRODUCT);
    expect(fixture.componentInstance.loading()).toBe(false);
  });
});
