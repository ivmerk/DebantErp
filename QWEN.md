# DebantERP - Project Context

## Project Overview

**DebantERP** is an enterprise resource planning (ERP) system built with **ASP.NET Core 9.0** using **DDD (Domain-Driven Design)** architecture. The system manages employees, specialties, production operations, orders, and labor costs for a manufacturing or service business.

### Tech Stack

- **Framework**: ASP.NET Core 9.0
- **Language**: C# (Nullable reference types enabled)
- **Database**: PostgreSQL 14 (via Dapper ORM)
- **Authentication**: Session-based + JWT support
- **Architecture**: DDD with Domain/Application/Infrastructure layers

### Key Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Dapper | 2.1.66 | Micro ORM for database access |
| Npgsql | 9.0.2 | PostgreSQL driver |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.2 | JWT authentication |
| Microsoft.AspNetCore.DataProtection | 9.0.0 | Session security |
| Microsoft.Extensions.Logging.Console | 9.0.2 | Logging |
| Swashbuckle.AspNetCore | 7.2.0 | Swagger/OpenAPI |

---

## Project Structure

```
DebantErp/
├── DebantErp/                      # Main application
│   ├── Domain/                     # Domain Layer (DDD)
│   │   ├── Base/                   # Entity, ValueObject, DomainEvent
│   │   ├── Sessions/               # AuthSession aggregate + VO
│   │   ├── Users/                  # User aggregate
│   │   ├── Enums/                  # Domain enums
│   │   ├── ValueObjects/           # Email, PersonName
│   │   └── Repositories/           # Repository interfaces
│   │
│   ├── Application/                # Application Layer (DDD)
│   │   └── Sessions/               # Session service + DTOs
│   │
│   ├── Infrastructure/             # Infrastructure Layer (DDD)
│   │   └── Repositories/           # Repository implementations
│   │
│   ├── BL/                         # Business Logic (Legacy)
│   │   ├── Auth/                   # Authentication
│   │   ├── Employee/               # Employee management
│   │   ├── Order/                  # Order management
│   │   ├── Speciality/             # Specialties
│   │   └── OrderLaborCost/         # Labor cost calculations
│   │
│   ├── DAL/                        # Data Access Layer
│   │   ├── Implementations/        # DAL implementations
│   │   ├── Interfaces/             # DAL interfaces
│   │   ├── Models/                 # DAL models
│   │   └── DbHelper.cs             # Dapper wrapper
│   │
│   ├── Controllers/                # MVC Controllers
│   ├── Db/                         # SQL migrations
│   ├── Dtos/                       # Data Transfer Objects
│   ├── Rdos/                       # Response Data Objects
│   ├── ViewModels/                 # View Models
│   ├── Views/                      # Razor views
│   ├── MockData/                   # Seed data
│   └── Tests/                      # Unit tests
│
├── DebantErpTest/                  # Test project (empty)
├── Doc/                            # Documentation
└── DebantErp.sln                   # Solution file
```

---

## Building and Running

### Prerequisites

- .NET 9.0 SDK
- PostgreSQL 14
- Docker (optional, for local development)

### Database Setup

1. **Start PostgreSQL** (using Docker):
```bash
cd DebantErp
docker-compose -f docker-compose.dev.yml up -d
```

2. **Run migrations** (located in `DebantErp/Db/`):
```bash
# Execute SQL files in order:
001_create_users.sql
002_create_employees.sql
003_create_specialities.sql
004_create_productuion_rates.sql
005_create_orders.sql
006_create_order_labor_costs.sql
007_create_auth_sessions.sql
```

### Run Application

```bash
cd /Users/ivanmerkulov/SynologyDrive/projects/DebantErp/DebantErp
dotnet run
```

**Default URLs:**
- Application: `https://localhost:7000` or `http://localhost:5000`
- pgAdmin: `http://localhost:8083`

### Build

```bash
dotnet build DebantErp/DebantErp.csproj
```

### Test

```bash
dotnet test DebantErpTest/DebantErpTest.csproj
```

---

## Architecture Patterns

### DDD Layers

**Domain Layer** (`Domain/`):
- Contains business entities, value objects, and domain events
- No dependencies on external frameworks
- Repository interfaces defined here
- Example: `AuthSession` aggregate with `SessionId`, `SessionToken` value objects

**Application Layer** (`Application/`):
- Application services orchestrate domain objects
- DTOs for data transfer
- No business logic, only coordination

**Infrastructure Layer** (`Infrastructure/`):
- Repository implementations
- External service implementations
- Database access via Dapper

**Business Logic Layer** (`BL/`):
- Legacy service layer (being refactored to DDD)
- Contains business rules and validators

### Key Domain Concepts

#### AuthSession (DDD Implementation)
```csharp
// Creation
var session = AuthSession.Create(userId, ipAddress, userAgent, rememberMe);

// Rehydration from DB
var session = AuthSession.Rehydrate(sessionId, userId, token, ...);

// Business operations
session.Refresh();
session.Revoke();
session.ExtendExpiration(newDate);
```

#### Domain Events
- `SessionCreatedEvent` - Raised when session is created
- `SessionExpiredEvent` - Raised when session expires
- `SessionRevokedEvent` - Raised when session is revoked

---

## Development Conventions

### Code Style

- **Nullable reference types**: Enabled
- **Implicit usings**: Enabled
- **Naming**: PascalCase for classes/methods, camelCase for local variables
- **File-scoped namespaces**: Used throughout

### Entity Pattern

```csharp
public class AuthSession : Entity, IAggregateRoot
{
    // Private setters for encapsulation
    public SessionId SessionId { get; private set; } = null!;
    
    // Factory methods
    public static AuthSession Create(...) { }
    public static AuthSession Rehydrate(...) { }
    
    // Business methods
    public void Refresh() { }
    public void Revoke() { }
}
```

### Repository Pattern

```csharp
// Interface in Domain
public interface IAuthSessionRepository
{
    Task<AuthSession?> GetByIdAsync(SessionId sessionId, ...);
    Task AddAsync(AuthSession session, ...);
    void Update(AuthSession session);
}

// Implementation in Infrastructure
public class AuthSessionRepository : IAuthSessionRepository
{
    // Uses Dapper for database access
}
```

### Logging

```csharp
using Microsoft.Extensions.Logging;

public class Service
{
    private readonly ILogger<Service> _logger;
    
    public Service(ILogger<Service> logger) => _logger = logger;
    
    public async Task Operation()
    {
        _logger.LogInformation("Operation started for {UserId}", userId);
    }
}
```

**Timestamp format**: `yyyy-MM-dd HH:mm:ss ` (local time)

---

## Database Schema

### Core Tables

| Table | Description |
|-------|-------------|
| `users` | User accounts (auth, roles) |
| `employees` | Employee records |
| `specialities` | Job specialties |
| `production_rates` | Production operation rates |
| `orders` | Customer orders |
| `order_labor_costs` | Labor cost tracking |
| `AuthSessions` | Authentication sessions |

### Session Table Schema

```sql
CREATE TABLE AuthSessions (
    SessionId uuid PRIMARY KEY,
    UserId int REFERENCES users(Id),
    SessionToken text UNIQUE,
    IpAddress varchar(45),
    UserAgent text,
    CreatedAt timestamp,
    LastAccessedAt timestamp,
    ExpiresAt timestamp,
    IsActive boolean
);
```

---

## Key Features

### Authentication
- Session-based authentication with SQL storage
- 30-minute idle timeout (default)
- Remember-me option (30-day sessions)
- Password hashing with salt

### User Roles
- **Admin**: Full access (user management, all operations)
- **User**: Limited access (own profile, employees, orders)

### Business Modules
1. **Employee Management** - CRUD operations for employees
2. **Specialties** - Job specialty definitions
3. **Production Rates** - Operation cost calculations
4. **Orders** - Order management
5. **Labor Costs** - Time tracking for orders

---

## Configuration

### Environment Variables (`.env`)

```bash
POSTGRES_USER=admin
POSTGRES_PASSWORD=test
POSTGRES_DB=debanterp
POSTGRES_PORT=5432
```

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Data Protection

Keys persisted to `.data-protection/` folder for session cookie encryption across restarts.

---

## Testing

### Test Structure
- Tests located in `DebantErp/Tests/` (co-located with code)
- Empty test project at `DebantErpTest/` (for future use)

### Running Tests
```bash
dotnet test
```

---

## Common Tasks

### Add New Migration
1. Create SQL file in `DebantErp/Db/` with next sequential number
2. Execute against database

### Add New Domain Entity
1. Create aggregate in `Domain/Entities/`
2. Create repository interface in `Domain/Repositories/`
3. Create service in `Application/`
4. Implement repository in `Infrastructure/`
5. Register in `Program.cs`

### Debug Session Issues
- Check `.data-protection/` folder for keys
- Clear browser cookies if key mismatch occurs
- Verify session table exists in database

---

## Notes

- **Session Storage**: Currently using PostgreSQL (not Redis) - suitable for single-server deployment
- **DDD Migration**: Project is being refactored from traditional BL/DAL to DDD architecture
- **Auth Sessions**: Fully implemented with DDD patterns (reference implementation)
- **Logging**: Configured with timestamps in local time format

---

## Related Documentation

- `technical_task.md` - Business requirements and domain description
- `Doc/` - Additional project documentation
