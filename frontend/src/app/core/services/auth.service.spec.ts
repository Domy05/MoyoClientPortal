import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthService, LoginResponse } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  const response: LoginResponse = {
    token: 'test-token',
    clientId: 'client-1',
    firstName: 'Test',
    lastName: 'Client',
    email: 'test@example.com',
    companyName: 'Test Company',
  };

  beforeEach(() => {
    localStorage.clear();
    sessionStorage.clear();

    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
    sessionStorage.clear();
  });

  it('starts logged out when no token exists', () => {
    expect(service.isLoggedIn()).toBe(false);
    expect(service.getToken()).toBeNull();
    expect(service.getClient()).toBeNull();
    expect(service.isOidcConfigured()).toBe(false);
  });

  it('logs in and stores the returned session', () => {
    service.login({ email: 'test@example.com', password: 'password' }).subscribe();

    const request = httpMock.expectOne('http://localhost:5141/api/auth/login');
    expect(request.request.method).toBe('POST');
    request.flush(response);

    expect(service.isLoggedIn()).toBe(true);
    expect(service.getToken()).toBe('test-token');
    expect(service.getClient()).toEqual({
      clientId: response.clientId,
      firstName: response.firstName,
      lastName: response.lastName,
      email: response.email,
      companyName: response.companyName,
    });
  });

  it('logs out and clears the stored session', () => {
    localStorage.setItem('moyo_auth_token', response.token);
    localStorage.setItem('moyo_client', JSON.stringify(response));
    service.isLoggedIn.set(true);

    service.logout();

    expect(service.isLoggedIn()).toBe(false);
    expect(service.getToken()).toBeNull();
    expect(service.getClient()).toBeNull();
  });

  it('rejects an invalid OIDC callback', async () => {
    await expect(service.handleOidcCallback()).rejects.toThrow(
      'Invalid OAuth 2.0/OpenID Connect callback.'
    );
  });
});
