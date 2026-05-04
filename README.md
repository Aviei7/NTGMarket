# BoardStoreProject

Web Application with ASP.NET Core and Angular.

The project combines a catalog API, client-side storefront UI, cart flow, checkout validation, promo-code support, and cookie-based authentication. It is set up for demo usage first: after the backend starts, it creates its local database automatically and fills it with seeded sample data.

## What the app does

- Browses a product catalog with categories, filters, pagination, and product details
- Stores a guest cart in cookies and syncs cart data through the backend
- Supports registration, login, logout, and loading the current user profile
- Validates checkout data and creates demo orders
- Applies promo codes from seeded loyalty data
- Exposes Swagger for API exploration

## Demo Notes

- The catalog content is seeded demo data, not production inventory
- Some assets, names, and product groups are mock or test-oriented
- Payment and delivery flows are simulated inside the app; there is no real external payment/shipping integration
- On backend startup, the seeder rebuilds the demo catalog and related demo tables so the project starts from a predictable state
- The account/cabinet area is only partially implemented

## Stack

- Backend: ASP.NET Core 8, EF Core, JWT auth in HttpOnly cookies, Serilog
- Frontend: Angular 20, Angular Material, PrimeNG, Bootstrap
- Runtime modes:
  - Local portfolio mode: SQLite + in-memory cache
  - Container mode: SQL Server + Redis via Docker Compose

## Quick Start

### Requirements

- .NET 8 SDK
- Node.js 20+
- npm

### 1. Run the backend

From the repository root:

```powershell
cd BoardGameStore
dotnet restore BoardGameStore.sln
dotnet run --project WebApp/WebApp.csproj
```

Backend URLs:

- API: `http://localhost:5090`
- Swagger: `http://localhost:5090/swagger`

What happens on first start:

- the backend creates a local SQLite database automatically
- EF initializes the schema
- the existing seeder fills the app with demo products, filters, promo codes, delivery/payment options, and other lookup data

No local SQL Server or Redis instance is required for this default mode.

### 2. Run the frontend

Open a second terminal:

```powershell
cd BoardGameStoreUI
npm install
npm start
```

Frontend URL:

- Storefront: `http://localhost:4200`

The Angular dev server proxies `/api`, `/images`, and `/swagger` requests to the backend.

## Docker Mode

If you want the full containerized setup instead of the lightweight local mode:

```powershell
docker compose up --build
```

This starts:

- Angular UI on `http://localhost:4200`
- ASP.NET Core API on `http://localhost:5090`
- SQL Server container
- Redis container

In Docker mode the backend switches to `SqlServer` + `Redis` through environment variables.

## Configuration

Default local configuration lives in [BoardGameStore/WebApp/appsettings.json](BoardGameStore/WebApp/appsettings.json).

Current defaults:

- `Database:Provider = Sqlite`
- `Cache:Provider = Memory`

You can override them with environment variables, for example:

```powershell
$env:Database__Provider = "SqlServer"
$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=BoardGameStore;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True;Encrypt=False;"
$env:Cache__Provider = "Redis"
$env:ConnectionStrings__Redis = "localhost:6379"
```

## Project Structure

- [BoardGameStore](BoardGameStore) - ASP.NET Core backend
- [BoardGameStoreUI](BoardGameStoreUI) - Angular frontend
- [docker-compose.yml](docker-compose.yml) - production-style container setup
- [docker-compose.dev.yml](docker-compose.dev.yml) - hot-reload development container setup
