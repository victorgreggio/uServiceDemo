# Blazor WebAssembly UI Guide

## Overview

This project includes a complete Blazor WebAssembly (WASM) UI for managing Weather Forecasts. The UI is a client-side single-page application that communicates with the backend API through an API Gateway.

## Architecture

```
┌─────────────────┐
│   Browser       │
│  (Blazor WASM)  │
└────────┬────────┘
         │ HTTP
         ▼
┌─────────────────┐
│  API Gateway    │
│   (YARP)        │
│  Port: 8081     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   API Service   │
│  (ASP.NET)      │
└─────────────────┘
```

## Features

### 1. List Weather Forecasts (`/weatherforecasts`)
- Displays all weather forecasts in a responsive table
- Shows: Date, Temperature (°C and °F), Summary, Created date, Last updated date, Updated by
- **Actions**: 
  - ✏️ Edit button for each forecast
  - 🗑️ Delete button with confirmation dialog
  - ➕ Create New button

### 2. Create Weather Forecast (`/weatherforecasts/create`)
- Form with validation:
  - **Date**: DateTime picker
  - **Temperature**: Numeric input (in Celsius)
  - **Summary**: Text input (3-100 characters)
- **Buttons**: 
  - Save (creates the forecast)
  - Cancel (returns to list)
- Real-time validation feedback
- Error handling with user-friendly messages

### 3. Edit Weather Forecast (`/weatherforecasts/edit/{id}`)
- Pre-populated form with existing forecast data
- Same validation as create form
- **Buttons**: 
  - Save (updates the forecast)
  - Cancel (returns to list)
- Error handling for not found and update failures

### 4. Delete Weather Forecast
- Confirmation dialog: "Are you sure you want to delete this weather forecast?"
- Integrated into the list view
- Error handling for failed deletions

## Project Structure

```
src/UI/
├── Pages/
│   ├── Home.razor                    # Landing page
│   ├── WeatherForecasts.razor        # List all forecasts (CRUD operations)
│   ├── CreateWeatherForecast.razor   # Create new forecast
│   ├── EditWeatherForecast.razor     # Edit existing forecast
│   ├── Counter.razor                 # Demo page (can be removed)
│   └── Weather.razor                 # Demo page (can be removed)
├── Services/
│   ├── IWeatherForecastService.cs    # Service interface
│   └── WeatherForecastService.cs     # HTTP client service implementation
├── Layout/
│   ├── MainLayout.razor              # Main layout wrapper
│   └── NavMenu.razor                 # Navigation menu
├── wwwroot/
│   ├── appsettings.json              # API base URL configuration
│   ├── css/                          # Stylesheets
│   └── index.html                    # SPA entry point
├── App.razor                         # Root component
├── Program.cs                        # Application entry point
└── _Imports.razor                    # Global using statements
```

## Configuration

### API Base URL

Configure the API Gateway URL in `src/UI/wwwroot/appsettings.json`:

```json
{
  "ApiBaseUrl": "http://localhost:8081"
}
```

This can be changed for different environments (dev, staging, production).

## Running the UI

### Option 1: Run Everything with .NET Aspire (Recommended)

This is the easiest way to run the entire application with all dependencies:

```bash
cd src/AppHost
dotnet run
```

This will start:
- ✅ API Service
- ✅ Worker Service
- ✅ UI (Blazor WASM)
- ✅ API Gateway (YARP) on port 8081
- ✅ PostgreSQL
- ✅ MongoDB
- ✅ Azure Service Bus (local emulator)
- ✅ Elasticsearch

Once running:
1. The console will display the **Aspire Dashboard** URL (typically `http://localhost:15XXX`)
2. Open the Aspire Dashboard in your browser
3. Navigate to the UI application from the dashboard
4. Or access the API Gateway directly at `http://localhost:8081`

### Option 2: Run UI Standalone (For UI Development)

If you're only working on the UI and the API is already running:

```bash
cd src/UI
dotnet run
```

**Note**: The API service must be running separately at `http://localhost:8081` for this to work.

### Option 3: Watch Mode (Auto-reload on changes)

For UI development with hot reload:

```bash
cd src/UI
dotnet watch run
```

This will automatically rebuild and reload the UI when you make changes to `.razor`, `.cs`, or `.css` files.

## Technology Stack

### Frontend Framework
- **Blazor WebAssembly** - .NET 9.0 running in the browser via WebAssembly
- **C#** - Programming language for UI logic
- **Razor** - Component syntax mixing HTML and C#

### UI Library
- **Bootstrap 5** - Responsive CSS framework
- **Bootstrap Icons** - Icon library

### HTTP Communication
- **HttpClient** - Native .NET HTTP client
- **Dependency Injection** - Service registration and injection
- **Async/Await** - Asynchronous operations

### Data Binding & Validation
- **Two-way binding** - `@bind` directive
- **DataAnnotations** - Validation attributes from Contracts project
- **EditForm** - Built-in form component with validation

## API Endpoints

The UI communicates with the following API endpoints through the gateway:

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/weatherforecast` | List all weather forecasts |
| GET | `/api/weatherforecast/{id}` | Get forecast by ID |
| POST | `/api/weatherforecast` | Create new forecast |
| POST | `/api/weatherforecast/{id}` | Update existing forecast |
| DELETE | `/api/weatherforecast/{id}` | Delete forecast |

## Services

### IWeatherForecastService / WeatherForecastService

The service layer abstracts HTTP communication and provides:

```csharp
Task<List<WeatherForecast>> GetAllAsync()
Task<WeatherForecast> GetByIdAsync(Guid id)
Task<WeatherForecast> CreateAsync(AddWeatherForecastRequest request)
Task<WeatherForecast> UpdateAsync(Guid id, UpdateWeatherForecastRequest request)
Task DeleteAsync(Guid id)
```

This is registered in `Program.cs`:
```csharp
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
```

## Routing

Navigation routes defined in the UI:

| Route | Page | Description |
|-------|------|-------------|
| `/` | Home.razor | Landing page |
| `/weatherforecasts` | WeatherForecasts.razor | List all forecasts |
| `/weatherforecasts/create` | CreateWeatherForecast.razor | Create new forecast |
| `/weatherforecasts/edit/{id}` | EditWeatherForecast.razor | Edit existing forecast |
| `/counter` | Counter.razor | Demo page |
| `/weather` | Weather.razor | Demo page |

## Next Steps / Enhancements

Potential improvements to the UI:

1. **Authentication/Authorization**: Add user login and role-based access
2. **Pagination**: Add pagination support for large datasets
3. **Filtering/Sorting**: Add filter and sort capabilities to the list view
4. **Loading States**: Add loading spinners/indicators
5. **Toast Notifications**: Add toast notifications for success/error messages
6. **Responsive Design**: Enhance mobile responsiveness
7. **Dark Mode**: Add dark/light theme toggle
8. **Export Functionality**: Export data to CSV/Excel
9. **Search**: Add search functionality
10. **Unit Tests**: Add unit tests using bUnit
11. **E2E Tests**: Add end-to-end tests using Playwright

## Troubleshooting

### UI Can't Connect to API

1. Check that the API Gateway is running on port 8081
2. Verify the `ApiBaseUrl` in `wwwroot/appsettings.json`
3. Check browser console for CORS errors
4. Ensure all services are running if using standalone mode

### Build Errors

1. Clean and rebuild the solution:
   ```bash
   dotnet clean
   dotnet build
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

### Hot Reload Not Working

1. Stop the application
2. Clean the bin/obj folders:
   ```bash
   cd src/UI
   rm -rf bin obj
   ```
3. Restart with `dotnet watch run`

## Documentation

For more detailed information, see:
- **[README.md](README.md)** - Overall project overview
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - System architecture details
- **[UI_PROJECT_SUMMARY.md](UI_PROJECT_SUMMARY.md)** - UI implementation summary
- **[src/UI/README.md](src/UI/README.md)** - UI-specific documentation

## Support

The Blazor WASM UI is fully integrated with the uServiceDemo microservice architecture and demonstrates:
- Clean separation of concerns
- Service layer pattern
- Async/await throughout
- Error handling
- Form validation
- CRUD operations
- RESTful API communication
- Modern responsive design

Enjoy building with Blazor! 🚀
