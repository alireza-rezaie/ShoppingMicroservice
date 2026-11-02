# Authentication Service

ASP.NET Core Identity and JWT authentication for the ShoppingMicroservice solution.

## Endpoints
- POST /api/auth/register — create a Customer account
- POST /api/auth/login — validate credentials and issue tokens
- POST /api/auth/refresh — rotate a refresh token
- POST /api/auth/logout — revoke a refresh token (requires a valid access token)

Access tokens default to 15 minutes and refresh tokens to 7 days. SQLite stores Identity users and hashed refresh tokens.

Set Jwt__Key to a random secret of at least 32 bytes in every non-development environment. Keep issuer, audience, and signing key identical in Authentication, Catalog, and Basket. Never use the checked-in development key in production.
