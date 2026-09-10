import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Order } from '../../core/models/order.model';
import { Dashboard } from './dashboard';

const ORDERS: Order[] = [
  {
    id: 'order-1',
    status: 'pending',
    createdAt: '2026-01-01T00:00:00Z',
    orderItems: [{ id: 'item-1', productId: '1', quantity: 2, unitPrice: 5 }],
  },
  {
    id: 'order-2',
    status: 'confirmed',
    createdAt: '2026-01-02T00:00:00Z',
    orderItems: [{ id: 'item-2', productId: '2', quantity: 1, unitPrice: 12 }],
  },
];

describe('Dashboard', () => {
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Dashboard],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should create', () => {
    const fixture = TestBed.createComponent(Dashboard);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should load recent orders and compute stats on init', () => {
    const fixture = TestBed.createComponent(Dashboard);
    fixture.detectChanges();

    const request = httpMock.expectOne('http://localhost:5141/api/orders');
    expect(request.request.method).toBe('GET');
    request.flush(ORDERS);

    const component = fixture.componentInstance;
    expect(component.recentOrders()).toEqual(ORDERS);
    expect(component.totalOrders()).toBe(2);
    expect(component.pendingOrders()).toBe(1);
  });
});
