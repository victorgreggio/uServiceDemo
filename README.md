# uServiceDemo

A comprehensive demonstration of micro-service architecture built using **AGTecFramework** packages and **.NET Aspire** orchestration framework.

## Overview

This project showcases a complete micro-service architecture implementation for managing Weather Forecast data. It demonstrates best practices in building distributed systems with modern .NET technologies, clean architecture principles, and event-driven design patterns.

## Architecture

The application follows a **micro-service architecture** with the following key components:

### System Components

```
┌─────────────────────────────────────────────────────────────────┐
│                       .NET Aspire AppHost                        │
│                     (Orchestration Layer)                        │
└─────────────────────────────────────────────────────────────────┘
                                │
                ┌───────────────┼───────────────┐
                │               │               │
                ▼               ▼               ▼
        ┌───────────┐   ┌──────────┐   ┌──────────────┐
        │    UI     │   │   API    │   │   Worker     │
        │  (Blazor  │   │  (ASP.   │   │  (Background │
        │   WASM)   │   │   NET)   │   │   Service)   │
        └───────────┘   └──────────┘   └──────────────┘
                │               │               │
                ▼               ▼               ▼
        ┌────────────────┐      ┌───────────────────┐
        │  API Gateway   │      │   Infrastructure  │
        │    (YARP)      │      │   Services        │
        └────────────────┘      └───────────────────┘
                                        │
                        ┌───────────────┼───────────────┬────────┐
                        │               │               │        │
                        ▼               ▼               ▼        ▼
                  ┌─────────┬──────┬────────┬──────────┐
                  │PostgreSQL│ Mongo│ Azure  │Elastic-  │
                  │   DB     │  DB  │Service │search    │
                  │          │      │  Bus   │          │
                  └─────────┴──────┴────────┴──────────┘
```

### Component Descriptions

1. **UI (Blazor WebAssembly)**: Client-side single-page application providing CRUD operations for Weather Forecasts
2. **API Service**: RESTful API built with ASP.NET Core Minimal APIs
3. **Worker Service**: Background service that processes events and maintains data consistency
4. **API Gateway (YARP)**: Reverse proxy that routes requests from the UI to the API service
5. **PostgreSQL**: Primary relational database for structured data storage
6. **MongoDB**: Document database for flexible data storage and querying
7. **Azure Service Bus**: Message broker for event-driven communication between services
8. **Elasticsearch**: Search and analytics engine for advanced querying capabilities

## AGTecFramework Integration

This project extensively uses the **AGTecFramework** suite of packages to implement enterprise-grade patterns and capabilities:

### Framework Packages Used

| Package | Purpose | Used In |
|---------|---------|---------|
| **AGTec.Common.Domain** | Domain entities and value objects | Domain layer |
| **AGTec.Common.Repository** | Repository pattern implementation | Infrastructure layer |
| **AGTec.Common.Repository.Document** | MongoDB repository abstractions | Document layer |
| **AGTec.Common.Repository.Search** | Elasticsearch repository abstractions | Document layer |
| **AGTec.Common.Document** | Document database utilities | Document layer |
| **AGTec.Common.CQRS** | CQRS pattern implementation | Application layer |
| **AGTec.Common.CQRS.Messaging.AzureServiceBus** | Azure Service Bus integration | Application layer |
| **AGTec.Common.CQRS.Messaging.ProtoBufSerializer** | Efficient message serialization | Application layer |
| **AGTec.Common.BackgroundTaskQueue** | Background task processing | Application & Worker |
| **AGTec.Services.ServiceDefaults** | Common service configurations | API service |

### Key Patterns Implemented

- **CQRS (Command Query Responsibility Segregation)**: Separates read and write operations for better scalability
- **Repository Pattern**: Abstracts data access logic
- **Use Case Pattern**: Encapsulates business logic in discrete use cases
- **Event-Driven Architecture**: Services communicate through events via Azure Service Bus
- **Clean Architecture**: Onion architecture with clear separation of concerns

## .NET Aspire Framework

The project uses **.NET Aspire** for service orchestration and management, providing:

### Aspire Benefits

- **Service Discovery**: Automatic service discovery and configuration
- **Resource Management**: Coordinated management of databases, message brokers, and search engines
- **Development Experience**: Simplified local development with unified dashboard
- **Configuration**: Centralized configuration management across services
- **Health Monitoring**: Built-in health checks and monitoring

### Aspire Configuration

The `AppHost` project orchestrates all services:

```csharp
// Infrastructure services
var postgres = builder.AddPostgres("Postgres").AddDatabase("WeatherforecastDB");
var mongodb = builder.AddMongoDB("MongoDB").AddDatabase("MongoWeatherforecastDocumentDB");
var azureServiceBus = builder.AddAzureServiceBus("AzureServiceBus");
var elasticsearch = builder.AddElasticsearch("Elasticsearch");

// API Service with dependencies
var api = builder.AddProject<Projects.uServiceDemo_Api>("api")
    .WithReference(postgres).WaitFor(postgres)
    .WithReference(mongodb).WaitFor(mongodb)
    .WithReference(azureServiceBus).WaitFor(azureServiceBus)
    .WithReference(elasticsearch).WaitFor(elasticsearch);

// Worker Service
builder.AddProject<Projects.uServiceDemo_Worker>("worker")
    .WithReference(postgres).WaitFor(postgres)
    .WithReference(mongodb).WaitFor(mongodb)
    .WithReference(azureServiceBus).WaitFor(azureServiceBus)
    .WithReference(elasticsearch).WaitFor(elasticsearch);

// UI and API Gateway
var ui = builder.AddProject<Projects.uServiceDemo_UI>("ui");
builder.AddYarp("ApiGateway")
    .WithHostPort(8081)
    .WithConfiguration(yarp => {
        yarp.AddRoute("/api/{**catch-all}", api)
            .WithTransformPathRemovePrefix("/api");
    })
    .WithReference(ui);
```

## Features

### Weather Forecast Management

- **List**: View all weather forecasts with pagination support
- **Create**: Add new weather forecasts with validation
- **Read**: Retrieve individual forecast details
- **Update**: Modify existing forecasts
- **Delete**: Remove forecasts with confirmation

### Data Flow Example

When a user creates a weather forecast:

1. **UI** sends POST request to API Gateway
2. **API Gateway (YARP)** routes to API Service
3. **API Service** invokes the `AddWeatherForecastUseCase`
4. **Use Case** dispatches `CreateWeatherForecastCommand`
5. **Command Handler** persists data to **PostgreSQL**
6. **Event** (`WeatherForecastCreatedEvent`) is published to **Azure Service Bus**
7. **Worker Service** receives event and updates **MongoDB** and **Elasticsearch**
8. **UI** receives success response and updates the display

## Technology Stack

### Frontend
- **Blazor WebAssembly** - Client-side .NET web framework
- **Bootstrap 5** - UI component library
- **HttpClient** - REST API communication

### Backend
- **ASP.NET Core 9.0** - Web API framework
- **Minimal APIs** - Lightweight endpoint definitions
- **Entity Framework Core** - ORM for PostgreSQL
- **AutoMapper** - Object-to-object mapping

### Data Storage
- **PostgreSQL** - Primary relational database
- **MongoDB** - Document store for flexible schemas
- **Elasticsearch** - Search and analytics

### Messaging & Events
- **Azure Service Bus** - Reliable message broker
- **ProtoBuf** - Efficient binary serialization

### Infrastructure
- **.NET Aspire** - Service orchestration
- **YARP** (Yet Another Reverse Proxy) - API Gateway

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for infrastructure services)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# Dev Kit

### Running the Application

#### Option 1: Using .NET Aspire (Recommended)

The easiest way to run the entire application with all dependencies:

```bash
cd src/AppHost
dotnet run
```

This starts:
- ✅ API Service
- ✅ Worker Service  
- ✅ UI (Blazor WASM)
- ✅ API Gateway (YARP)
- ✅ PostgreSQL
- ✅ MongoDB
- ✅ Azure Service Bus (local emulator)
- ✅ Elasticsearch

Once running, access:
- **Aspire Dashboard**: Displayed in console output (typically `http://localhost:15XXX`)
- **API Gateway**: `http://localhost:8081`
- **UI Application**: Accessible through Aspire Dashboard or API Gateway

#### Option 2: Running Services Individually

**API Service:**
```bash
cd src/Api
dotnet run
```

**Worker Service:**
```bash
cd src/Worker
dotnet run
```

**UI (Development):**
```bash
cd src/UI
dotnet run
```

> **Note**: When running individually, ensure infrastructure services (PostgreSQL, MongoDB, Azure Service Bus, Elasticsearch) are running separately.

### Configuration

#### API Connection (UI)

Configure the API base URL in `src/UI/wwwroot/appsettings.json`:

```json
{
  "ApiBaseUrl": "http://localhost:8081"
}
```

#### Database Connections

Connection strings are managed by Aspire. For manual configuration, update `appsettings.json` in the respective service projects.

## Project Structure

```
uServiceDemo/
├── src/
│   ├── Api/                    # REST API endpoints
│   ├── AppHost/                # .NET Aspire orchestration
│   ├── Application/            # Business logic & use cases
│   │   ├── Commands/           # CQRS commands
│   │   ├── CommandHandlers/    # Command handlers
│   │   ├── Queries/            # CQRS queries
│   │   ├── QueryHandlers/      # Query handlers
│   │   └── UseCases/           # Business use cases
│   ├── Contracts/              # Shared DTOs and requests
│   ├── Document/               # MongoDB & Elasticsearch repos
│   ├── Domain/                 # Domain entities and enums
│   ├── Events/                 # Event definitions
│   ├── Infrastructure/         # PostgreSQL repos & EF Core
│   ├── UI/                     # Blazor WASM frontend
│   └── Worker/                 # Background event processor
├── test/                       # Unit and integration tests
├── ARCHITECTURE.md             # Detailed architecture documentation
└── README.md                   # This file
```

## API Endpoints

The API exposes the following endpoints through the gateway at `http://localhost:8081`:

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/weatherforecast` | List all weather forecasts |
| GET | `/api/weatherforecast/{id}` | Get forecast by ID |
| POST | `/api/weatherforecast` | Create new forecast |
| POST | `/api/weatherforecast/{id}` | Update existing forecast |
| DELETE | `/api/weatherforecast/{id}` | Delete forecast |

## Key Concepts Demonstrated

### 1. Micro-Service Architecture
- Independent, loosely-coupled services
- Each service has its own data store
- Communication through events and APIs

### 2. Event-Driven Design
- Services publish events on state changes
- Asynchronous processing through message broker
- Eventual consistency across services

### 3. CQRS Pattern
- Separate models for reading and writing data
- Optimized query paths
- Scalable command processing

### 4. Clean Architecture
- Domain-centric design
- Dependency inversion
- Technology-agnostic business logic

### 5. API Gateway Pattern
- Single entry point for clients
- Request routing and transformation
- Simplified client configuration

## Development

### Building the Solution

```bash
dotnet build uServiceDemo.sln
```

### Running Tests

```bash
dotnet test
```

### Database Migrations

Entity Framework Core migrations are in the `Infrastructure` project:

```bash
cd src/Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../Api
dotnet ef database update --startup-project ../Api
```

## Documentation

- **[ARCHITECTURE.md](ARCHITECTURE.md)**: Detailed system architecture and data flows
- **[OAUTH_IMPLEMENTATION.md](OAUTH_IMPLEMENTATION.md)**: OAuth/OIDC authentication guide
- **[OAUTH_CONFIGURATION_GUIDE.md](OAUTH_CONFIGURATION_GUIDE.md)**: Configure identity providers
- **[OAUTH_SETUP_SUMMARY.md](OAUTH_SETUP_SUMMARY.md)**: Implementation summary
- **[OAUTH_QUICK_REFERENCE.md](OAUTH_QUICK_REFERENCE.md)**: Quick reference card
- **[UI_PROJECT_SUMMARY.md](UI_PROJECT_SUMMARY.md)**: UI implementation details
- **[BLAZOR_UI_GUIDE.md](BLAZOR_UI_GUIDE.md)**: Blazor UI development guide

## Security

### Authentication & Authorization

This project implements **OAuth 2.0 / OpenID Connect (OIDC)** authentication using **Duende Software's demo identity server**.

**Key Features:**
- ✅ OAuth 2.0 Authorization Code Flow with PKCE
- ✅ JWT Bearer token authentication for API
- ✅ Secure token storage in browser
- ✅ All API endpoints protected with `[Authorize]`
- ✅ Login/Logout functionality in UI
- ✅ Automatic token refresh

**Configuration:**
- **Authority**: `https://demo.duendesoftware.com`
- **Client ID**: `interactive.public`
- **Scopes**: `openid`, `profile`, `api`
- **Test Credentials**: `alice`/`alice` or `bob`/`bob`

For detailed information, see [OAUTH_IMPLEMENTATION.md](OAUTH_IMPLEMENTATION.md).

## Future Enhancements

- [x] Authentication and authorization (JWT/OAuth2) ✅ **Implemented**
- [ ] API versioning
- [ ] Rate limiting and throttling
- [ ] Distributed tracing (OpenTelemetry)
- [ ] API documentation (Swagger/OpenAPI)
- [ ] Unit and integration test coverage
- [ ] CI/CD pipeline
- [ ] Containerization (Docker Compose)
- [ ] Kubernetes deployment manifests
- [ ] Performance monitoring and metrics

## Contributing

This is a demonstration project showcasing micro-service architecture patterns. Feel free to use it as a reference for building your own micro-service applications with AGTecFramework and .NET Aspire.

## License

This project is a demonstration and learning resource.

## Acknowledgments

- **AGTecFramework**: For providing comprehensive enterprise-grade packages
- **.NET Aspire**: For simplifying distributed application development
- **Microsoft**: For the excellent .NET ecosystem

---

**Built with ❤️ using .NET 9.0, AGTecFramework, and .NET Aspire**
