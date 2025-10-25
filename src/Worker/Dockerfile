# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Build arguments for private NuGet feed
ARG NUGET_USERNAME
ARG NUGET_PASSWORD

# Copy solution file
COPY ["uServiceDemo.sln", "./"]

# Copy project files
COPY ["src/Worker/uServiceDemo.Worker.csproj", "src/Worker/"]
COPY ["src/Application/uServiceDemo.Application.csproj", "src/Application/"]
COPY ["src/Contracts/uServiceDemo.Contracts.csproj", "src/Contracts/"]
COPY ["src/Domain/uServiceDemo.Domain.csproj", "src/Domain/"]
COPY ["src/Events/uServiceDemo.Events.csproj", "src/Events/"]
COPY ["src/Infrastructure/uServiceDemo.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Document/uServiceDemo.Document.csproj", "src/Document/"]

# Configure private NuGet source with credentials
RUN dotnet nuget add source "https://nuget.pkg.github.com/victorgreggio/index.json" \
    --name "GitHub" \
    --username "${NUGET_USERNAME}" \
    --password "${NUGET_PASSWORD}" \
    --store-password-in-clear-text

# Restore dependencies
RUN dotnet restore "src/Worker/uServiceDemo.Worker.csproj"

# Copy source code
COPY ["src/Worker/", "src/Worker/"]
COPY ["src/Application/", "src/Application/"]
COPY ["src/Contracts/", "src/Contracts/"]
COPY ["src/Domain/", "src/Domain/"]
COPY ["src/Events/", "src/Events/"]
COPY ["src/Infrastructure/", "src/Infrastructure/"]
COPY ["src/Document/", "src/Document/"]

# Build
WORKDIR "/src/src/Worker"
RUN dotnet build "uServiceDemo.Worker.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "uServiceDemo.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "uServiceDemo.Worker.dll"]
