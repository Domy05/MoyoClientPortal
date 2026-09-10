# Moyo Client Portal — Solution Architecture

## Scope and implementation choice

The implemented system is the **Client Portal**. It allows clients to authenticate, browse products, view orders, manage a cart, and create new orders. The Product Management System and Order Management System are external solution boundaries described by integration interfaces and adapters; they are not implemented as separate applications in this submission.

## Overall solution

```text
Client
  │ HTTPS
  ▼
Angular Client Portal
  │ HTTPS/JSON, OAuth 2.0/OIDC PKCE
  ▼
ASP.NET Core Client Portal API
  ├── SQL Server / Azure SQL: clients, products, carts, orders, order items
  ├── Product System: synchronous product reads
  ├── Order Management Platform: asynchronous new-order delivery
  └── Order Management webhook: asynchronous status updates
```

The Client Portal owns the client experience, carts, orders, and order history. The Product System is the source of product information. The Order Management System owns downstream vendor allocation and operational order processing.

## Synchronous product flow

1. The Angular client requests the product list or a product detail.
2. The API authenticates the request and calls `IProductSystemClient`.
3. When `ProductSystem:BaseUrl` is configured, the HTTP client calls the external Product System.
4. The API maps the response into the portal product DTO and returns JSON to Angular.
5. If the external URL is intentionally empty for local development, the adapter reads the local database fallback.

This is synchronous because the caller waits for the product response before rendering the page.

## Asynchronous order flow

1. The client submits a new order to the portal API.
2. The API validates the client, products, quantities, and prices, then stores the order with a pending status.
3. The order is placed on the order-management channel and the API can return without waiting for vendor allocation.
4. `OrderManagementWorker` delivers the order event to the external Order Management Platform when `OrderManagement:BaseUrl` is configured.
5. The Order Management Platform posts status events to `POST /api/integrations/order-management/status`.
6. The status event is validated with the webhook key, queued, and processed by `OrderStatusUpdateWorker`.
7. The worker updates the order status in the portal database.

This decouples customer order submission from downstream processing and allows retries or temporary external-system outages without blocking the client request.

## Data model

The completed ERD is the authoritative database diagram. Its core entities are:

- `Clients`: portal users and customer identity.
- `Products`: product catalogue records used by the portal fallback and order lines.
- `CartItems`: a client's current product selections.
- `Orders`: order header, owner, status, and creation metadata.
- `OrderItems`: products and quantities captured at order time.

`Orders` belongs to one `Client` and has many `OrderItems`. Each `OrderItem` references one `Product`. A `Client` can have many `CartItems`, and each `CartItem` references one `Product`. Junction entities are used instead of unnecessary direct many-to-many relationships.

## Security

- OAuth 2.0/OpenID Connect authorization-code flow with PKCE is used by the browser integration.
- The API validates issuer, audience, lifetime, and signing keys for OIDC tokens.
- Local JWT login is retained as a development/demo fallback.
- API calls carry bearer tokens through the Angular interceptor.
- The order-management status endpoint uses a separate webhook API key.
- HTTPS is required in deployment and HTTP is redirected to HTTPS.
- Secrets belong in PaaS application settings or GitHub secrets, never in source control.
- CORS allows only the deployed frontend origin rather than every origin.
- Production should use a randomly generated JWT signing key, restricted database access, managed identities where available, and log redaction for tokens and credentials.

## Scalability and reliability

- The Angular frontend is static and can be served from an edge/CDN platform.
- The API is stateless apart from the database and can be scaled horizontally on App Service.
- EF Core database access uses scoped contexts and indexed relational tables.
- Product and order integrations are behind interfaces, allowing independent replacement and testing.
- Background workers prevent slow external systems from blocking order submission.
- Status updates are processed asynchronously and can be extended with retry, dead-letter, idempotency, and correlation IDs.
- Production monitoring should include API availability, failed authentication, order queue depth, integration latency, webhook failures, and database health.

## Deployment and CI/CD

GitHub is the source-control and automation layer. GitHub Actions restores dependencies, builds the .NET API, builds the Angular production bundle, and deploys the two artifacts to Azure App Service and Azure Static Web Apps. Azure SQL provides the managed relational database. Configuration is injected through deployment settings and repository variables.

A health endpoint at `/health` provides a lightweight deployment smoke test. The deployment workflow is manual by default; it can be changed to deploy on pushes to `main` after branch protection and review are enabled.

## Important design decision

The submission implements the Client Portal only, while preserving clear boundaries to the Product System and Order Management System. The empty-URL adapters are useful for local development and automated tests; they are not evidence of a live enterprise integration. A production demonstration must configure the external URLs or explicitly label the adapters as mocks in the submission.
