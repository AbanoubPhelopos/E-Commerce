FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/basket/Basket.API/Basket.API.csproj", "services/basket/Basket.API/"]
COPY ["services/basket/Basket.Application/Basket.Application.csproj", "services/basket/Basket.Application/"]
COPY ["services/basket/Basket.Core/Basket.Core.csproj", "services/basket/Basket.Core/"]
COPY ["services/basket/Basket.Infrastructure/Basket.Infrastructure.csproj", "services/basket/Basket.Infrastructure/"]

RUN dotnet restore "./services/basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/services/basket/Basket.API"
RUN dotnet build "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
