import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { Order } from '../../../core/models/order.model';
import { Product } from '../../../core/models/product.model';
import { SearchService } from '../../../core/services/search.service';
import { OrderList } from './order-list';

const PRODUCTS: Product[] = [
  {
    id: 'product-1',
    name: 'Notebook',
    price: 5,
    stock: 10,
    category: 'Paper',
    image: 'notebook.jpg',
  },
];

const ORDERS: Order[] = Array.from({ length: 6 }, (_, index) => ({
  id: `order-${index + 1}`,
  status: index === 0 ? 'pending' : index === 1 ? 'shipped' : 'confirmed',
  createdAt: `2026-01-0${index + 1}T00:00:00Z`,
  orderItems: [
    {
      id: `item-${index + 1}`,
      productId: 'product-1',
      quantity: index + 1,
      unitPrice: 5,
    },
  ],
}));

describe('OrderList', () => {
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderList],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  function loadOrders() {
    const fixture = TestBed.createComponent(OrderList);
    fixture.detectChanges();
    httpMock.expectOne('http://localhost:5141/api/products').flush(PRODUCTS);
    httpMock.expectOne('http://localhost:5141/api/orders').flush(ORDERS);
    return fixture.componentInstance;
  }

  it('should create', () => {
    const fixture = TestBed.createComponent(OrderList);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('loads products and orders on init', () => {
    const component = loadOrders();

    expect(component.products()).toEqual(PRODUCTS);
    expect(component.orders()).toEqual(ORDERS);
    expect(component.loading()).toBe(false);
    expect(component.totalPages()).toBe(2);
    expect(component.paginatedOrders()).toHaveLength(5);
  });

  it('filters by status and search text', () => {
    const component = loadOrders();

    component.selectStatus('shipped');
    expect(component.filteredOrders()).toHaveLength(1);
    expect(component.getStatusLabel('confirmed')).toBe('Processing');
    expect(component.getStatusLabel('delivered')).toBe('delivered');

    component.selectStatus('all');
    TestBed.inject(SearchService).setSearchTerm('ORD-ORDER-2');
    expect(component.filteredOrders()).toHaveLength(1);
  });

  it('calculates totals, paginates, and navigates to an order', () => {
    const component = loadOrders();
    const router = TestBed.inject(Router);
    const navigateSpy = vi.spyOn(router, 'navigate');

    expect(component.getOrderNumber(ORDERS[0])).toBe('ORD-ORDER-1');
    expect(component.getOrderTotal(ORDERS[1])).toBe(10);

    component.nextPage();
    expect(component.currentPage()).toBe(2);
    expect(component.paginatedOrders()).toHaveLength(1);
    component.nextPage();
    expect(component.currentPage()).toBe(2);
    component.previousPage();
    expect(component.currentPage()).toBe(1);
    component.previousPage();
    expect(component.currentPage()).toBe(1);

    component.viewOrder(ORDERS[0]);
    expect(navigateSpy).toHaveBeenCalledWith(['/orders', 'order-1']);
  });
});
