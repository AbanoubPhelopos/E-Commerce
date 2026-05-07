FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["apiGateways/Ocelot.APIGateway/Ocelot.APIGateway.csproj", "apiGateways/Ocelot.APIGateway/"]
RUN dotnet restore "./apiGateways/Ocelot.APIGateway/Ocelot.APIGateway.csproj"
COPY . .
WORKDIR "/src/apiGateways/Ocelot.APIGateway"
RUN dotnet build "./Ocelot.APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ocelot.APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/apiGateways/Ocelot.APIGateway/ocelot.Docker.json ./ocelot.Development.json
ENTRYPOINT ["dotnet", "Ocelot.APIGateway.dll"]
