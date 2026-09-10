import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthService, LoginResponse } from './auth.service';
import { CartItem, CartService } from './cart.service';

describe('CartService', () => {
  const client: LoginResponse = {
    token: 'token',
    clientId: 'client-1',
    firstName: 'Test',
    lastName: 'Client',
    email: 'test@example.com',
    companyName: 'Test Company',
  };

  const item: CartItem = {
    productId: 'product-1',
    name: 'Notebook',
    price: 5,
    quantity: 2,
    image: 'notebook.jpg',
  };

  function createService(currentClient: LoginResponse | null) {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: AuthService,
          useValue: { getClient: () => currentClient },
        },
      ],
    });

    return {
      service: TestBed.inject(CartService),
      httpMock: TestBed.inject(HttpTestingController),
    };
  }

  afterEach(() => TestBed.inject(HttpTestingController).verify());

  it('stays empty when the user is not logged in', () => {
    const { service } = createService(null);

    expect(service.items()).toEqual([]);
    expect(service.itemCount()).toBe(0);
    expect(service.loading()).toBe(false);
  });

  it('loads the logged-in user cart', () => {
    const { service, httpMock } = createService(client);
    const request = httpMock.expectOne('http://localhost:5141/api/cart/client-1');
    request.flush([item]);

    expect(service.items()).toEqual([item]);
    expect(service.itemCount()).toBe(2);
    expect(service.loading()).toBe(false);
  });

  it('adds an item and reloads the cart', () => {
    const { service, httpMock } = createService(client);
    httpMock.expectOne('http://localhost:5141/api/cart/client-1').flush([]);

    service.addItem(
      { productId: item.productId, name: item.name, price: item.price, image: item.image },
      3
    );

    const addRequest = httpMock.expectOne('http://localhost:5141/api/cart/client-1');
    expect(addRequest.request.method).toBe('POST');
    expect(addRequest.request.body).toEqual({ productId: 'product-1', quantity: 3 });
    addRequest.flush({ message: 'Item added to cart.' });

    httpMock.expectOne('http://localhost:5141/api/cart/client-1').flush([item]);
    expect(service.items()).toEqual([item]);
  });

  it('updates quantity, removes an item, and clears the cart', () => {
    const { service, httpMock } = createService(client);
    httpMock.expectOne('http://localhost:5141/api/cart/client-1').flush([item]);

    service.increaseQuantity('product-1');
    const increaseRequest = httpMock.expectOne(
      'http://localhost:5141/api/cart/client-1/product-1'
    );
    expect(increaseRequest.request.method).toBe('PUT');
    expect(increaseRequest.request.body).toEqual({ quantity: 3 });
    increaseRequest.flush({ message: 'Cart item updated.' });
    httpMock.expectOne('http://localhost:5141/api/cart/client-1').flush([item]);

    service.decreaseQuantity('product-1');
    const decreaseRequest = httpMock.expectOne(
      'http://localhost:5141/api/cart/client-1/product-1'
    );
    expect(decreaseRequest.request.body).toEqual({ quantity: 1 });
    decreaseRequest.flush({ message: 'Cart item updated.' });
    httpMock.expectOne('http://localhost:5141/api/cart/client-1').flush([item]);

    service.removeItem('product-1');
    const removeRequest = httpMock.expectOne(
      'http://localhost:5141/api/cart/client-1/product-1'
    );
    expect(removeRequest.request.method).toBe('DELETE');
    removeRequest.flush({ message: 'Item removed from cart.' });
    httpMock.expectOne('http://localhost:5141/api/cart/client-1').flush([]);

    service.clear();
    const clearRequest = httpMock.expectOne('http://localhost:5141/api/cart/client-1');
    expect(clearRequest.request.method).toBe('DELETE');
    clearRequest.flush({ message: 'Cart cleared.' });

    expect(service.items()).toEqual([]);
  });
});
