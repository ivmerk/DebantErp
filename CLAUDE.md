# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

**DebantERP** — ERP system for managing employees, specialties, production operations, orders, and labor costs. Built with ASP.NET Core 9.0 MVC, PostgreSQL via Dapper.

## Commands

```bash
# Start the PostgreSQL database (from DebantErp/ directory)
docker-compose -f docker-compose.dev.yml up -d

# Run the application (from DebantErp/ directory)
dotnet run

# Build
dotnet build DebantErp.sln

# Run tests
dotnet test DebantErpTest/DebantErpTest.csproj
```

App runs at `http://localhost:5010` in the `http` profile.  
pgAdmin is available at `http://localhost:8083` (email: `ivan.e.merkulov@gmail.com`, password: `test`).

**SQL migrations must be run manually** against the database in order: `Db/001_*.sql` through `Db/007_*.sql`. There is no migration runner.

## Architecture

The project is in the middle of a migration from a flat BL/DAL pattern to DDD. Both patterns coexist:

### Legacy BL/DAL pattern (most of the codebase)
- `DAL/Interfaces/` + `DAL/Implementations/` — raw Dapper queries, all going through `DbHelper.cs`
- `BL/` — service classes with business logic, each paired with an interface (`IEmployee`/`Employee`, `IOrder`/`Order`, etc.)
- `Controllers/` → `BL/` → `DAL/` — the standard request path

`DbHelper` is a static wrapper with three methods (`ExecuteAsync`, `ExecuteScalarAsync`, `QueryAsync`) that open a new `NpgsqlConnection` per call. **The connection string is hardcoded in `DbHelper.cs`**, not read from `appsettings.json`.

### DDD layers (target pattern — no live reference yet)
The intended DDD shape is `Domain/` (aggregates, value objects, domain events, repository interfaces — no framework deps) → `Application/` (services orchestrating domain objects + command DTOs) → `Infrastructure/Repositories/` (Dapper repo implementations). These folders do not exist yet; the former `AuthSession` reference implementation was never built out and the session feature now uses plain cookie auth (see Authentication).

When adding new domain features, follow the DDD pattern: domain aggregate → repository interface in `Domain/Repositories/` → service in `Application/` → repository impl in `Infrastructure/` → register in `Program.cs`.

### Data flow conventions
- `Dtos/` — inbound request objects (Create/Update)
- `Rdos/` — outbound response objects
- `DAL/Models/` — internal DB row models (never expose directly to controllers)
- `ViewModels/` — Razor view binding models (Login, Register, Profile)

### Authentication
Cookie authentication (`AddAuthentication().AddCookie()`, scheme `Cookies`). On login `Auth.Authenticate` verifies the password and calls `HttpContext.SignInAsync` with a `ClaimsPrincipal` carrying `NameIdentifier` (userid), `Name` (email), and `Role`. `/logout` (POST) calls `SignOutAsync`. The server is stateless — there is no server-side session store and no DB-backed session table. `ICurrentUser.IsLoggedIn()` just checks `User.Identity.IsAuthenticated`.

Data protection keys are persisted to `.data-protection/` so the auth cookie survives restarts/redeploys. If cookie decryption fails after a key rotation, clear browser cookies.

New users are created with `Status = NeedToApprove`; admins must activate accounts. Password hashing uses SHA + per-user salt (see `BL/Auth/Encrypt.cs`).

**Authorization is deny-by-default**: `Program.cs` sets a `FallbackPolicy` requiring an authenticated user, so every endpoint needs login unless marked `[AllowAnonymous]` (`Home`, `Login`, `Register`, and `/health` via `.AllowAnonymous()`). `UserController` is `[Authorize(Roles = "Admin")]`. The role claim value is the `UserRoleEnum` name (`"Admin"`/`"User"`, capitalized) — role checks are case-sensitive, so match that casing in `[Authorize(Roles = ...)]`.

### Mock data
`MockDataSeeder.SeedAsync()` is called unconditionally at application startup (`Program.cs`). It seeds users, employees, specialties, orders, and production rates if the tables are empty.

### Roles
- `admin` — full access including user management
- `user` — CRUD on employees, specialties, operations, rates, orders, labor costs; own profile only

## Key files
- `Program.cs` — DI registration and startup; see here to understand all registered services
- `DAL/DbHelper.cs` — all DB calls go through this; hardcoded connection string lives here
- `BL/Auth/Auth.cs` — login/logout via cookie sign-in; `CurrentUser.cs` — auth-state check
- `Db/*.sql` — numbered migration scripts; apply in order
