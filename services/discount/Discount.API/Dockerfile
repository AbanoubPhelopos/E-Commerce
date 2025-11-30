FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/discount/Discount.API/Discount.API.csproj", "services/discount/Discount.API/"]
COPY ["services/discount/Discount.Application/Discount.Application.csproj", "services/discount/Discount.Application/"]
COPY ["services/discount/Discount.Core/Discount.Core.csproj", "services/discount/Discount.Core/"]
COPY ["services/discount/Discount.Infrastructure/Discount.Infrastructure.csproj", "services/discount/Discount.Infrastructure/"]

RUN dotnet restore "./services/discount/Discount.API/Discount.API.csproj"
COPY . .
WORKDIR "/src/services/discount/Discount.API"
RUN dotnet build "./Discount.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Discount.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.API.dll"]
