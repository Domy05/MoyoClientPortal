# Moyo Client Portal

Angular client portal and ASP.NET Core API for the Online Order Solution case study.

## Scope

This repository implements the Client Portal option:

- Client login
- Product browsing
- Cart management
- Order creation
- Order browsing and details
- Synchronous Product System boundary
- Asynchronous Order Management boundary and status webhook

The architecture and ERD documentation are in:

- [`docs/solution-architecture.md`](docs/solution-architecture.md)
- [`docs/production-deployment.md`](docs/production-deployment.md)

## Local run

Requirements: .NET 10 SDK, Node.js 20+, npm, and a local SQL Server instance using the development connection string.

```bash
npm install
npm --prefix frontend install
npm run dev
```

Open `http://localhost:4200`.

## Production

The prepared Microsoft-focused deployment uses GitHub Actions, Azure App Service, Azure Static Web Apps, and Azure SQL. Configure the required GitHub secrets, repository variables, OIDC provider, database connection string, CORS origin, and external Product/Order Management URLs before running the workflow. See [`docs/production-deployment.md`](docs/production-deployment.md).

The frontend uses build-time environment configuration and no longer hardcodes production API URLs. The backend applies EF Core migrations at startup, exposes `/health`, and requires explicit production CORS configuration.
