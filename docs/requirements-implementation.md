# Requirements implementation

This solution now contains the following architecture boundaries:

- Angular frontend under `frontend/`.
- OAuth 2.0/OpenID Connect authorization-code-with-PKCE support in the Angular client, configurable in `frontend/src/app/core/services/oidc-config.ts`.
- ASP.NET Core Web API in C# under `backend/`.
- EF Core and SQL Server through `ClientPortalDbContext`; the existing database and migrations are preserved.
- Synchronous Product System boundary through `IProductSystemClient`. Set `ProductSystem:BaseUrl` to call an external Product System; leave it empty for the local database fallback.
- Asynchronous new-order boundary through `IOrderManagementPlatform` and `OrderManagementWorker`. Set `OrderManagement:BaseUrl` to send queued orders to an external Order Management Platform.
- Asynchronous inbound status updates through `POST /api/integrations/order-management/status`, processed by `OrderStatusUpdateWorker`.
- Azure PaaS deployment workflow in `.github/workflows/azure-deploy.yml`.

The local in-memory adapter keeps development runnable without external accounts or credentials. Production deployment must configure the identity provider, Product System URL, Order Management URL, webhook key, database connection string, and Azure GitHub secrets through deployment settings rather than committing secrets to source control.

See [Solution Architecture](solution-architecture.md) for the complete architecture, synchronous/asynchronous flows, security, scalability, and deployment decisions. See [Production Deployment](production-deployment.md) for the deployment checklist and required settings.
