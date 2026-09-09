import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  clientId: string;
  firstName: string;
  lastName: string;
  email: string;
  companyName: string;
}

const TOKEN_KEY = 'moyo_auth_token';
const CLIENT_KEY = 'moyo_client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);

  isLoggedIn = signal<boolean>(
    !!localStorage.getItem(TOKEN_KEY)
  );

  login(credentials: LoginRequest) {
    return this.http
      .post<LoginResponse>(
        'http://localhost:5141/api/auth/login',
        credentials
      )
      .pipe(
        tap(response => {
          localStorage.setItem(
            TOKEN_KEY,
            response.token
          );

          localStorage.setItem(
            CLIENT_KEY,
            JSON.stringify({
              clientId: response.clientId,
              firstName: response.firstName,
              lastName: response.lastName,
              email: response.email,
              companyName: response.companyName
            })
          );

          this.isLoggedIn.set(true);
        })
      );
  }

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(CLIENT_KEY);

    this.isLoggedIn.set(false);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  getClient(): LoginResponse | null {
    const client = localStorage.getItem(CLIENT_KEY);

    if (!client) {
      return null;
    }

    return JSON.parse(client);
  }
}