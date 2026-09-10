import { provideHttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { SearchService } from '../../core/services/search.service';
import { Layout } from './layout';

describe('Layout', () => {
  beforeEach(async () => {
    localStorage.clear();

    await TestBed.configureTestingModule({
      imports: [Layout],
      providers: [provideRouter([]), provideHttpClient()],
    }).compileComponents();
  });

  afterEach(() => localStorage.clear());

  it('should create with search disabled on an unrelated route', () => {
    const fixture = TestBed.createComponent(Layout);
    const component = fixture.componentInstance;

    expect(component).toBeTruthy();
    expect(component.currentPage).toBe('disabled');
    expect(component.searchEnabled).toBe(false);
    expect(component.searchPlaceholder).toBe('Search unavailable');
  });

  it('enables and clears search for products and orders routes', () => {
    const fixture = TestBed.createComponent(Layout);
    const component = fixture.componentInstance;
    const searchService = TestBed.inject(SearchService);

    component.updateSearchState('/products');
    expect(component.currentPage).toBe('products');
    expect(component.searchEnabled).toBe(true);
    expect(component.searchPlaceholder).toBe('Search products...');

    component.onSearch({ target: { value: 'notebook' } } as unknown as Event);
    expect(searchService.searchTerm()).toBe('notebook');

    component.updateSearchState('/orders');
    expect(component.currentPage).toBe('orders');
    expect(component.searchPlaceholder).toBe('Search orders...');
    expect(searchService.searchTerm()).toBe('');
  });

  it('returns client display information', () => {
    localStorage.setItem('moyo_client', JSON.stringify({
      clientId: 'client-1',
      firstName: 'Jane',
      lastName: 'Doe',
      email: 'jane@example.com',
      companyName: 'Example Co',
    }));

    const component = TestBed.createComponent(Layout).componentInstance;

    expect(component.client?.email).toBe('jane@example.com');
    expect(component.initials).toBe('JD');
    expect(component.fullName).toBe('Jane Doe');
  });

  it('navigates to login when logged out', () => {
    const component = TestBed.createComponent(Layout).componentInstance;
    const router = TestBed.inject(Router);
    const navigateSpy = vi.spyOn(router, 'navigate');

    component.onAuthClick();

    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });

  it('logs out and navigates to login when logged in', () => {
    localStorage.setItem('moyo_auth_token', 'token');
    const component = TestBed.createComponent(Layout).componentInstance;
    const authService = TestBed.inject(AuthService);
    const logoutSpy = vi.spyOn(authService, 'logout');
    const router = TestBed.inject(Router);
    const navigateSpy = vi.spyOn(router, 'navigate');

    component.onAuthClick();

    expect(logoutSpy).toHaveBeenCalled();
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });
});
