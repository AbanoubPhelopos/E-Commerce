FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/ordering/Ordering.Api/Ordering.Api.csproj", "services/ordering/Ordering.Api/"]
COPY ["services/ordering/Ordering.Application/Ordering.Application.csproj", "services/ordering/Ordering.Application/"]
COPY ["services/ordering/Ordering.Core/Ordering.Core.csproj", "services/ordering/Ordering.Core/"]
COPY ["services/ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "services/ordering/Ordering.Infrastructure/"]
COPY ["infrastructure/EventBus.Messages/EventBus.Messages.csproj", "infrastructure/EventBus.Messages/"]

RUN dotnet restore "./services/ordering/Ordering.Api/Ordering.Api.csproj"
COPY . .
WORKDIR "/src/services/ordering/Ordering.Api"
RUN dotnet build "./Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.Api.dll"]
