# Shopping Microservices

A .NET 9 microservices sample containing Catalog, Basket, Discount, and Authentication services.

## Authentication

The Authentication API uses the same four-layer API / Application / Core / Infrustructure structure as the other services, with ASP.NET Core Identity, SQL Server, JWT access tokens, and rotating refresh tokens. Its SQL Server database is persisted in the Docker named volume `auth_sql_data`.

Run the solution with Docker Compose:

```bash
docker compose up --build
```

Authentication API: `http://localhost:9002`  
Swagger UI: `http://localhost:9002/swagger`  
Authentication SQL Server: `localhost,1433` (database: `ShoppingAuthDb`)

### Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/v1/Auth/Register` | Register a Customer and issue tokens |
| POST | `/api/v1/Auth/Login` | Sign in and issue tokens |
| POST | `/api/v1/Auth/Refresh` | Rotate a refresh token |
| POST | `/api/v1/Auth/Logout` | Revoke the supplied refresh token |

Example registration body:

```json
{
  "email": "customer@example.com",
  "password": "StrongPass123",
  "fullName": "Sample Customer"
}
```

The response contains an access token (15-minute default) and a refresh token (7-day default). Send the access token to protected APIs using:

```
Authorization: Bearer <access-token>
```

Basket endpoints require authentication and only allow the signed-in email to access its basket. Catalog reads remain public; creating, updating, and deleting products require authentication.

### Local development and secrets

The credentials in `docker-compose.override.yml` and the checked-in JWT key are for local development only. Replace them with strong secrets before deployment. Provide the SQL Server connection string and JWT signing key through a secret manager or environment variables in deployed environments. Configure the same JWT issuer, audience, and signing key in Authentication, Catalog, and Basket.

### Database scope

- Authentication: SQL Server
- Catalog: MongoDB (unchanged)
- Basket: Redis (unchanged)

The Authentication API currently creates its initial schema with EF Core `EnsureCreated`. For production schema evolution, add EF Core migrations and use `Database.MigrateAsync()`.

### Verification

Build and Docker execution have not been run in this authoring environment. Verify locally with:

```bash
dotnet build ShoppingMicroservice.sln
docker compose up --build
```
