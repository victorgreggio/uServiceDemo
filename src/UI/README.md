# uServiceDemo.UI

This is a Blazor WebAssembly (WASM) UI project for managing Weather Forecasts.

## Features

- **List Weather Forecasts**: View all weather forecasts in a table
- **Create Weather Forecast**: Add new weather forecasts with date, temperature, and summary
- **Edit Weather Forecast**: Modify existing weather forecasts
- **Delete Weather Forecast**: Remove weather forecasts from the system

## Configuration

The API base URL can be configured in `wwwroot/appsettings.json`:

```json
{
  "ApiBaseUrl": "http://localhost:8081"
}
```

By default, the UI expects the API Gateway to be running at `http://localhost:8081`.

## Running the Application

### Standalone Mode

To run the UI project standalone:

```bash
cd src/UI
dotnet run
```

The application will be available at `http://localhost:5039` (or `https://localhost:7274`).

### With .NET Aspire

The recommended way is to run the entire solution using .NET Aspire's AppHost:

```bash
cd src/AppHost
dotnet run
```

This will orchestrate all services including:
- API
- Worker
- UI
- PostgreSQL
- MongoDB
- Azure Service Bus
- Elasticsearch
- API Gateway (YARP)

## Project Structure

- `Pages/` - Blazor pages/components
  - `WeatherForecasts.razor` - List view
  - `CreateWeatherForecast.razor` - Create form
  - `EditWeatherForecast.razor` - Edit form
- `Services/` - HTTP client services
  - `IWeatherForecastService.cs` - Service interface
  - `WeatherForecastService.cs` - Service implementation
- `Layout/` - Layout components
- `wwwroot/` - Static assets and configuration

## Dependencies

- Microsoft.AspNetCore.Components.WebAssembly (9.0.9)
- uServiceDemo.Contracts - Shared data contracts

## API Endpoints Used

The UI communicates with the following API endpoints:

- `GET /api/weatherforecast` - List all forecasts
- `GET /api/weatherforecast/{id}` - Get forecast by ID
- `POST /api/weatherforecast` - Create new forecast
- `POST /api/weatherforecast/{id}` - Update existing forecast
- `DELETE /api/weatherforecast/{id}` - Delete forecast
