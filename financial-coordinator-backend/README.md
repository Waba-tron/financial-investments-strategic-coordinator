# FinancialInvestmentStrategicCoordinator — Backend


## Projects

| Project | Role |
|---|---|
| `FinancialInvestmentStrategicCoordinator.Domain` | Entities, enums, constants. No dependencies. |
| `FinancialInvestmentStrategicCoordinator.Application` | CQRS (MediatR), interfaces, DTOs, FluentValidation validators, exceptions. |
| `FinancialInvestmentStrategicCoordinator.Infrastructure` | EF Core (SQLite), JWT, BCrypt, global exception middleware, seeding. |
| `FinancialInvestmentStrategicCoordinator.API` | Controllers, `Program.cs`, wiring. Thin — controllers only `IMediator.Send`. |

## Run

```bash
dotnet build FinancialInvestmentStrategicCoordinator.slnx
dotnet run --project FinancialInvestmentStrategicCoordinator.API
# Swagger: http://localhost:5041/swagger
```

The database (`FinancialInvestmentStrategicCoordinator.db`) is created with `EnsureCreated` on
startup and seed users are inserted (passwords BCrypt-hashed at runtime).

> **Note:** `EnsureCreated` is a dev convenience and is incompatible with EF migrations. To move to
> migrations, swap `EnsureCreatedAsync` for `MigrateAsync` in `DatabaseSeeder` and add an initial
> migration with `dotnet ef migrations add Initial`.

## Seed users

| Email | Password | Role |
|---|---|---|
| `admin@fisco.local` | `Admin1234` | Admin |
| `customer@fisco.local` | `Password123` | Customer |

Self-registration (`POST /api/auth/register`) assigns the default role **Customer**.

## Endpoints

| Method | Route | Access |
|---|---|---|
| POST | `/api/auth/register` | anonymous |
| POST | `/api/auth/login` | anonymous |
| GET | `/api/products`, `/api/products/{id}` | anonymous |
| POST/PUT/DELETE | `/api/products[/{id}]` | `Admin` |

## Error shape

All errors return the standard JSON body (camelCase): `message`, `errorCode`, `statusCode`,
optional `validationErrors` (PascalCase property keys), `timestamp`.

## Before production

- Move `Jwt:Key` and the connection string to user-secrets / environment variables.
- Prefer EF migrations over `EnsureCreated`.
- Access-token only — no refresh tokens / revocation (see template "Known gaps").
