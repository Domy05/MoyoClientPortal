# Production deployment

## Recommended Microsoft-focused deployment

The assessment asks for a public-cloud PaaS solution. The repository is prepared for this deployment shape:

```text
GitHub → GitHub Actions
              ├── Angular → Azure Static Web Apps
              └── ASP.NET Core → Azure App Service
                                      └── Azure SQL Database
```

The Product Management System and Order Management System are represented by integration interfaces and adapters in this Client Portal. The in-memory adapters are intentionally retained as local/demo fallbacks; production uses the configured external URLs.

## Required production settings

Configure these as Azure App Service application settings. Do not commit values to `appsettings.json`.

```text
ConnectionStrings__DefaultConnection
Jwt__Key
Jwt__Issuer
Jwt__Audience
Authentication__Oidc__Authority
Authentication__Oidc__Audience
Cors__AllowedOrigins
ProductSystem__BaseUrl
OrderManagement__BaseUrl
OrderManagement__StatusWebhookApiKey
```

`Cors__AllowedOrigins` should contain the exact HTTPS frontend URL, for example:

```text
https://moyo-client-portal.azurestaticapps.net
```

The frontend build is configured through GitHub repository variables. Add:

```text
AZURE_BACKEND_URL   = https://your-api.azurewebsites.net
OIDC_AUTHORITY      = https://login.microsoftonline.com/<tenant-id>/v2.0
OIDC_CLIENT_ID      = <public SPA client id>
OIDC_SCOPE          = openid profile email api://<api-app-id>/access_as_user
OIDC_API_AUDIENCE   = api://<api-app-id>
```

The OIDC client ID is public browser configuration, not a secret. The JWT signing key, database password, webhook key, and Azure credentials must be GitHub/Azure secrets.

## First deployment checklist

1. Create an Azure SQL database and copy its connection string.
2. Create an Azure App Service running .NET 10.
3. Create an Azure Static Web App.
4. Register an Entra ID/OIDC SPA client and API application. Add the deployed frontend URL plus `/auth/callback` as a redirect URI.
5. Configure the API audience and authority in App Service settings.
6. Configure the Azure SQL connection string and production secrets in App Service settings.
7. Add GitHub secret `AZURE_CREDENTIALS`.
8. Add GitHub secret `AZURE_BACKEND_APP_NAME`.
9. Add GitHub secret `AZURE_STATIC_WEB_APPS_API_TOKEN`.
10. Add the frontend repository variables listed above.
11. Run the `Azure PaaS Deployment` workflow from GitHub Actions.
12. Verify `https://your-api.azurewebsites.net/health` returns `{ "status": "ok" }`.
13. Test OIDC login, product browsing, cart operations, order creation, and status updates from the public frontend.

The EF Core migrations are committed under `backend/Migrations`. The application applies pending migrations during startup before seeding demo data. For a production business database, replace demo seed data with an approved data-loading process before opening the application to real customers.

## Public demo limitations

- Local email/password login remains available as a fallback for development and demo accounts.
- OIDC is the production authentication path when `Authentication:Oidc` and the frontend OIDC variables are configured.
- Product calls remain local only when `ProductSystem:BaseUrl` is empty.
- New orders use the local in-memory adapter only when `OrderManagement:BaseUrl` is empty.
- These fallbacks must be documented to a reviewer and replaced with real system URLs for an end-to-end integration demonstration.
