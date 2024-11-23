FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore --configfile NuGet.Config

FROM build-env AS build-api
WORKDIR /app/src/Api
RUN dotnet publish -c Release -o /app/out

FROM build-env AS build-worker
WORKDIR /app/src/Worker
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS worker
WORKDIR /app
COPY --from=build-worker /app/out .
ENTRYPOINT ["dotnet", "uServiceDemo.Worker.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app
COPY --from=build-api /app/out .
ENTRYPOINT ["dotnet", "uServiceDemo.Api.dll"]
