import { environment } from '../../../environments/environment';

export interface OidcConfig {
  authority: string;
  clientId: string;
  scope: string;
  redirectUri: string;
  apiAudience?: string;
}

// Set authority and clientId for the selected OAuth 2.0/OpenID Connect
// provider in the deployment configuration. Empty values keep the local
// development login available until a provider is configured.

export const oidcConfig: OidcConfig = {
  authority: environment.oidc.authority,
  clientId: environment.oidc.clientId,
  scope: environment.oidc.scope,
  redirectUri: `${window.location.origin}/auth/callback`,
  apiAudience: environment.oidc.apiAudience,
};
