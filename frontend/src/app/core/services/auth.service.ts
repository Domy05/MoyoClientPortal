import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { oidcConfig } from './oidc-config';
import { environment } from '../../../environments/environment';

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

interface OidcDiscoveryDocument {
  authorization_endpoint: string;
  token_endpoint: string;
}

const TOKEN_KEY = 'moyo_auth_token';
const CLIENT_KEY = 'moyo_client';
const OIDC_STATE_KEY = 'moyo_oidc_state';
const OIDC_VERIFIER_KEY = 'moyo_oidc_verifier';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);

  isLoggedIn = signal<boolean>(!!localStorage.getItem(TOKEN_KEY));

  isOidcConfigured(): boolean {
    return Boolean(oidcConfig.authority && oidcConfig.clientId);
  }

  login(credentials: LoginRequest) {
    return this.http
      .post<LoginResponse>(
        `${environment.apiBaseUrl}/auth/login`,
        credentials
      )
      .pipe(tap((response) => this.storeSession(response)));
  }

  async startOidcLogin(): Promise<void> {
    if (!this.isOidcConfigured()) {
      return;
    }

    const discovery = await this.getDiscoveryDocument();
    const state = this.randomString(32);
    const verifier = this.randomString(64);
    const challenge = await this.createCodeChallenge(verifier);

    sessionStorage.setItem(OIDC_STATE_KEY, state);
    sessionStorage.setItem(OIDC_VERIFIER_KEY, verifier);

    const params = new URLSearchParams({
      client_id: oidcConfig.clientId,
      response_type: 'code',
      redirect_uri: oidcConfig.redirectUri,
      scope: oidcConfig.scope,
      state,
      code_challenge: challenge,
      code_challenge_method: 'S256',
    });

    window.location.assign(`${discovery.authorization_endpoint}?${params}`);
  }

  async handleOidcCallback(): Promise<void> {
    const query = new URLSearchParams(window.location.search);
    const code = query.get('code');
    const state = query.get('state');
    const expectedState = sessionStorage.getItem(OIDC_STATE_KEY);
    const verifier = sessionStorage.getItem(OIDC_VERIFIER_KEY);

    if (!code || !state || state !== expectedState || !verifier) {
      throw new Error('Invalid OAuth 2.0/OpenID Connect callback.');
    }

    const discovery = await this.getDiscoveryDocument();
    const body = new URLSearchParams({
      grant_type: 'authorization_code',
      client_id: oidcConfig.clientId,
      code,
      redirect_uri: oidcConfig.redirectUri,
      code_verifier: verifier,
    });

    const response = await fetch(discovery.token_endpoint, {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body,
    });

    if (!response.ok) {
      throw new Error('OAuth 2.0/OpenID Connect token exchange failed.');
    }

    const tokens = (await response.json()) as {
      access_token: string;
      id_token?: string;
    };

    const claims = tokens.id_token ? this.decodeJwtPayload(tokens.id_token) : {};

    this.storeSession({
      token: tokens.access_token,
      clientId: String(claims['sub'] ?? ''),
      firstName: String(claims['given_name'] ?? ''),
      lastName: String(claims['family_name'] ?? ''),
      email: String(claims['email'] ?? claims['preferred_username'] ?? ''),
      companyName: String(claims['company_name'] ?? ''),
    });

    sessionStorage.removeItem(OIDC_STATE_KEY);
    sessionStorage.removeItem(OIDC_VERIFIER_KEY);
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

    return JSON.parse(client) as LoginResponse;
  }

  private storeSession(response: LoginResponse): void {
    localStorage.setItem(TOKEN_KEY, response.token);
    localStorage.setItem(
      CLIENT_KEY,
      JSON.stringify({
        clientId: response.clientId,
        firstName: response.firstName,
        lastName: response.lastName,
        email: response.email,
        companyName: response.companyName,
      })
    );
    this.isLoggedIn.set(true);
  }

  private async getDiscoveryDocument(): Promise<OidcDiscoveryDocument> {
    const response = await fetch(
      `${oidcConfig.authority.replace(/\/$/, '')}/.well-known/openid-configuration`
    );

    if (!response.ok) {
      throw new Error('Unable to load the OpenID Connect discovery document.');
    }

    return (await response.json()) as OidcDiscoveryDocument;
  }

  private randomString(length: number): string {
    const bytes = new Uint8Array(length);
    crypto.getRandomValues(bytes);
    return this.toBase64Url(bytes);
  }

  private async createCodeChallenge(verifier: string): Promise<string> {
    const data = new TextEncoder().encode(verifier);
    const digest = await crypto.subtle.digest('SHA-256', data);
    return this.toBase64Url(new Uint8Array(digest));
  }

  private toBase64Url(bytes: Uint8Array): string {
    let binary = '';
    bytes.forEach((byte) => (binary += String.fromCharCode(byte)));
    return btoa(binary)
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=+$/, '');
  }

  private decodeJwtPayload(token: string): Record<string, unknown> {
    const payload = token.split('.')[1];
    const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
    return JSON.parse(decoded) as Record<string, unknown>;
  }
}
