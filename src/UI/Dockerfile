# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Build arguments for private NuGet feed
ARG NUGET_USERNAME
ARG NUGET_PASSWORD

# Copy solution file
COPY ["uServiceDemo.sln", "./"]

# Copy project files
COPY ["src/UI/uServiceDemo.UI.csproj", "src/UI/"]
COPY ["src/Contracts/uServiceDemo.Contracts.csproj", "src/Contracts/"]

# Configure private NuGet source with credentials
RUN dotnet nuget add source "https://nuget.pkg.github.com/victorgreggio/index.json" \
    --name "GitHub" \
    --username "${NUGET_USERNAME}" \
    --password "${NUGET_PASSWORD}" \
    --store-password-in-clear-text

# Restore dependencies
RUN dotnet restore "src/UI/uServiceDemo.UI.csproj"

# Copy source code
COPY ["src/UI/", "src/UI/"]
COPY ["src/Contracts/", "src/Contracts/"]

# Build and publish
WORKDIR "/src/src/UI"
RUN dotnet publish "uServiceDemo.UI.csproj" -c Release -o /app/publish

# Runtime stage - nginx for static content
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy published Blazor WASM files
COPY --from=build /app/publish/wwwroot .

# Remove default nginx config and copy custom one
RUN rm /etc/nginx/conf.d/default.conf
COPY src/UI/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
