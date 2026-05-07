# Ocelot API Gateway

API Gateway built with Ocelot, routing external requests to internal microservices.

## Overview

| Property | Value |
|-----------|-------|
| Port | 8010 |
| Framework | ASP.NET Core 10.0 |
| Gateway Library | Ocelot 24.1.0 |

## Architecture

```
Client → Ocelot Gateway (8010) → Catalog API (8080)
                                → Basket API (8081)
                                → Discount API (8082)
                                → Ordering API (8083)
```

## Routes

| Upstream | Downstream | Target Service |
|----------|------------|----------------|
| `/Catalog/**` | `/api/v1/Catalog/**` | Catalog API |
| `/Basket/**` | `/api/v1/Basket/**` | Basket API |
| `/Discount/**` | `/api/v1/Discount/**` | Discount API |
| `/Order/**` | `/api/v1/Order/**` | Ordering API |

## Features

- **Rate Limiting** — Basket Checkout endpoints limited to 1 req/3s
- **Caching** — Catalog GET routes cached for 30 seconds
- **API aggregation** — Single entry point for all services

## Ocelot Config

The gateway loads environment-specific config:
- Development: `ocelot.Development.json` (uses `host.docker.internal`)
- Docker: `ocelot.Docker.json` (uses container names)

## Run

```bash
# Outside Docker (with services running via Docker)
dotnet run --project apiGateways/Ocelot.APIGateway

# Inside Docker Compose
cd services/docker-compose && docker-compose up -d
```

---

See [README.md](../../README.md) for full project documentation.