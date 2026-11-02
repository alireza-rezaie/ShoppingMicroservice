# Authentication Service

The Authentication microservice follows the same four-project structure as Catalog and Basket:

- **Authentication.Api** — versioned ASP.NET Core controllers, Swagger, JWT middleware, and dependency registration.
- **Authentication.Application** — MediatR commands, handlers, and response DTOs.
- **Authentication.Core** — domain entities and repository/service abstractions.
- **Authentication.Infrustructure** — ASP.NET Core Identity, SQL Server persistence, repository and JWT token service implementations.

Dependency direction follows the existing services: API -> Application / Infrastructure, Infrastructure -> Application, Application -> Core. Core has no project dependencies.

## Endpoints

All endpoints use the shared API route convention `api/v{version}/[controller]/[action]`:

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/v1/Auth/Register` | Register a Customer and issue tokens |
| POST | `/api/v1/Auth/Login` | Sign in and issue tokens |
| POST | `/api/v1/Auth/Refresh` | Rotate a refresh token |
| POST | `/api/v1/Auth/Logout` | Revoke the supplied refresh token (Bearer token required) |

Authentication data is stored in SQL Server database `ShoppingAuthDb`. Run the solution using `docker compose up --build`. SQL Server is exposed at `localhost,1433`; the API is exposed at `http://localhost:9002`.

JWT access tokens default to 15 minutes; refresh tokens default to 7 days. Refresh tokens are stored as SHA-256 hashes. Catalog MongoDB and Basket Redis remain unchanged.

The checked-in SQL Server password and JWT signing key are local-development values only. Replace them with strong secrets before deployment. Development schema creation currently uses EF Core `EnsureCreated`; introduce EF Core migrations before production schema evolution.

## Verification

Run `dotnet build ShoppingMicroservice.sln` and `docker compose up --build`. Build and container execution have not been run in the authoring environment.
