FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/catalog/Catalog.API/Catalog.API.csproj", "services/catalog/Catalog.API/"]
COPY ["services/catalog/Catalog.Application/Catalog.Application.csproj", "services/catalog/Catalog.Application/"]
COPY ["services/catalog/Catalog.Core/Catalog.Core.csproj", "services/catalog/Catalog.Core/"]
COPY ["services/catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "services/catalog/Catalog.Infrastructure/"]
COPY services/catalog/Catalog.Infrastructure/Data/SeedData /src/services/catalog/Catalog.Infrastructure/Data/SeedData
RUN dotnet restore "./services/catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/services/catalog/Catalog.API"
RUN dotnet build "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
