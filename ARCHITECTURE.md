# uServiceDemo Architecture

## System Architecture with New UI

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
                │       ┌───────┴───────┐       │
                │       │               │       │
                ▼       ▼               ▼       ▼
        ┌────────────────┐      ┌───────────────────┐
        │  API Gateway   │      │   Infrastructure  │
        │    (YARP)      │      │   Services        │
        │  Port: 8081    │      └───────────────────┘
        └────────────────┘              │
                │                       │
                │               ┌───────┼────────┬────────┐
                │               │       │        │        │
                │               ▼       ▼        ▼        ▼
                │         ┌─────────┬──────┬────────┬──────────┐
                │         │ Postgres│ Mongo│ Azure  │ Elastic  │
                │         │   DB    │  DB  │Service │ search   │
                │         │         │      │  Bus   │          │
                │         └─────────┴──────┴────────┴──────────┘
                │
                └──────► /api/weatherforecast endpoints
```

## UI Application Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    Blazor WASM UI                            │
│                  (Client-Side SPA)                           │
└─────────────────────────────────────────────────────────────┘
        │
        │ User Interaction
        ▼
┌─────────────────────────────────────────────────────────────┐
│                   Razor Components                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │WeatherFore-  │  │  Create      │  │   Edit       │     │
│  │casts.razor   │  │  Form        │  │   Form       │     │
│  │(List View)   │  │              │  │              │     │
│  └──────────────┘  └──────────────┘  └──────────────┘     │
└─────────────────────────────────────────────────────────────┘
        │
        │ Dependency Injection
        ▼
┌─────────────────────────────────────────────────────────────┐
│              IWeatherForecastService                         │
│           (Abstraction Layer)                                │
└─────────────────────────────────────────────────────────────┘
        │
        │ HttpClient
        ▼
┌─────────────────────────────────────────────────────────────┐
│         HTTP Requests to API Gateway                         │
│                                                               │
│  GET    /api/weatherforecast          (List All)            │
│  GET    /api/weatherforecast/{id}     (Get By ID)           │
│  POST   /api/weatherforecast          (Create)              │
│  POST   /api/weatherforecast/{id}     (Update)              │
│  DELETE /api/weatherforecast/{id}     (Delete) [NEW]        │
└─────────────────────────────────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────────────────────────────────┐
│                      API Gateway (YARP)                      │
│               Routes: /api/* → API Service                   │
└─────────────────────────────────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────────────────────────────────┐
│                      API Service                             │
│                   (Endpoints.cs)                             │
└─────────────────────────────────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────────────────────────────────┐
│                      Use Cases                               │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │
│  │ List        │  │  Get        │  │  Add        │        │
│  │ Weather     │  │  Weather    │  │  Weather    │        │
│  │ Forecasts   │  │  Forecast   │  │  Forecast   │        │
│  └─────────────┘  └─────────────┘  └─────────────┘        │
│  ┌─────────────┐  ┌─────────────┐                          │
│  │ Update      │  │  Delete     │ [NEW]                    │
│  │ Weather     │  │  Weather    │                          │
│  │ Forecast    │  │  Forecast   │                          │
│  └─────────────┘  └─────────────┘                          │
└─────────────────────────────────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────────────────────────────────┐
│              Command/Query Handlers                          │
│         (CQRS Pattern Implementation)                        │
└─────────────────────────────────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────────────────────────────────┐
│               Repositories & Data Access                     │
└─────────────────────────────────────────────────────────────┘
        │
        ▼
┌─────────────────────────────────────────────────────────────┐
│              PostgreSQL & MongoDB                            │
└─────────────────────────────────────────────────────────────┘
```

## Component Relationships

### UI Project Dependencies

```
uServiceDemo.UI
    │
    ├── Microsoft.AspNetCore.Components.WebAssembly (9.0.9)
    │
    └── uServiceDemo.Contracts (Project Reference)
            │
            └── Shared DTOs:
                ├── WeatherForecast
                ├── AddWeatherForecastRequest
                └── UpdateWeatherForecastRequest
```

### API Project Dependencies (Enhanced)

```
uServiceDemo.Api
    │
    ├── Endpoints.cs [MODIFIED]
    │   └── Added: MapDelete("/{id:guid}", ...) [NEW]
    │
    └── Uses:
        └── IDeleteWeatherForecastUseCase [NEW]
```

### Application Layer (Enhanced)

```
uServiceDemo.Application
    │
    ├── UseCases/
    │   ├── ListWeatherForecasts/V1/
    │   ├── GetWeatherForecast/V2/
    │   ├── AddWeatherForecast/V1/
    │   ├── UpdateWeatherForecast/V1/
    │   └── DeleteWeatherForecast/V1/ [NEW]
    │       ├── IDeleteWeatherForecastUseCase.cs
    │       └── DeleteWeatherForecastUseCase.cs
    │
    ├── Commands/
    │   ├── CreateWeatherForecastCommand.cs
    │   ├── UpdateWeatherForecastCommand.cs
    │   └── DeleteWeatherForecastCommand.cs [NEW]
    │
    └── CommandHandlers/
        ├── CreateWeatherForecastCommandHandler.cs
        ├── UpdateWeatherForecastCommandHandler.cs
        └── DeleteWeatherForecastCommandHandler.cs [NEW]
```

### Events Layer (Enhanced)

```
uServiceDemo.Events
    │
    ├── WeatherForecastCreatedEvent.cs
    ├── WeatherForecastUpdatedEvent.cs
    └── WeatherForecastDeletedEvent.cs [NEW]
```

## Data Flow Examples

### Create Weather Forecast Flow

```
User fills form → CreateWeatherForecast.razor
    ↓
WeatherForecastService.CreateAsync()
    ↓
POST /api/weatherforecast
    ↓
API Gateway (YARP) → API Service
    ↓
AddWeatherForecastUseCase.Execute()
    ↓
CreateWeatherForecastCommand
    ↓
CreateWeatherForecastCommandHandler
    ↓
Repository.Create()
    ↓
PostgreSQL Database
    ↓
WeatherForecastCreatedEvent published
    ↓
Azure Service Bus
```

### Delete Weather Forecast Flow [NEW]

```
User clicks Delete → Confirmation Dialog
    ↓
WeatherForecastService.DeleteAsync()
    ↓
DELETE /api/weatherforecast/{id}
    ↓
API Gateway (YARP) → API Service
    ↓
DeleteWeatherForecastUseCase.Execute()
    ↓
GetWeatherForecastByIdQuery (validation)
    ↓
DeleteWeatherForecastCommand
    ↓
DeleteWeatherForecastCommandHandler
    ↓
Repository.Delete()
    ↓
PostgreSQL Database
    ↓
WeatherForecastDeletedEvent published
    ↓
Azure Service Bus
```

## Technology Stack

### Frontend
- **Framework**: Blazor WebAssembly (WASM)
- **UI Library**: Bootstrap 5
- **HTTP Client**: System.Net.Http.HttpClient
- **Data Binding**: Two-way binding with @bind
- **Validation**: DataAnnotations
- **Routing**: Blazor Router

### Backend
- **API**: ASP.NET Core Minimal APIs
- **Architecture**: Clean Architecture / Onion Architecture
- **Patterns**: CQRS, Repository, Use Case
- **Event Bus**: Azure Service Bus
- **Serialization**: ProtoBuf
- **Mapping**: AutoMapper
- **Gateway**: YARP (Yet Another Reverse Proxy)

### Data Storage
- **Relational DB**: PostgreSQL
- **Document DB**: MongoDB
- **Search**: Elasticsearch

### Orchestration
- **.NET Aspire**: Service orchestration and management
- **Container Orchestration**: Docker (implied by Aspire)

## Security Considerations

Current implementation uses:
- HTTP (development)
- No authentication/authorization (to be implemented)
- CORS configuration needed for production
- API Gateway provides single entry point

Future enhancements should include:
- JWT or OAuth2 authentication
- Role-based authorization
- HTTPS enforcement
- API rate limiting
- Input sanitization
- XSS protection
