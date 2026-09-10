// This file is overwritten by the production deployment workflow.
// Do not put client secrets here: Angular configuration is public in the browser.
export const environment = {
  production: true,
  apiBaseUrl: 'https://replace-with-your-backend.azurewebsites.net/api',
  oidc: {
    authority: '',
    clientId: '',
    scope: 'openid profile email',
    apiAudience: '',
  },
};
