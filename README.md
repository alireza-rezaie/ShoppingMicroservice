# Shopping Microservices

A .NET 9 microservices sample containing Catalog, Basket, Discount, and Authentication services.

## Authentication

The Authentication API uses ASP.NET Core Identity, SQLite, JWT access tokens, and rotating refresh tokens.

Run the solution with Docker Compose:

```bash
docker compose up --build
```

Authentication API: `http://localhost:9002`  
Swagger UI: `http://localhost:9002/swagger`

### Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/auth/register` | Register a Customer and issue tokens |
| POST | `/api/auth/login` | Sign in and issue tokens |
| POST | `/api/auth/refresh` | Rotate a refresh token |
| POST | `/api/auth/logout` | Revoke the supplied refresh token |

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

The checked-in key in `appsettings.Development.json` and `docker-compose.override.yml` is for local development only. Before deployment, remove/override it and provide a cryptographically random `Jwt__Key` of at least 32 bytes through a secret manager or environment variable. Configure the same issuer, audience, and signing key in Authentication, Catalog, and Basket.

The SQLite database is stored in the Docker named volume `auth_data`.

### Notes

- Customer is assigned automatically at registration.
- Admin role authorization can be added to product-management actions after provisioning trusted admin accounts.
- This branch has not been built or integration-tested in this environment; run `dotnet build ShoppingMicroservice.sln` and `docker compose up --build` locally before merging.
