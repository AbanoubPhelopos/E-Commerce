# E-Commerce Microservices

A distributed e-commerce platform built with ASP.NET microservices, following Clean Architecture principles.

## Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                    E-Commerce Solution                                │
├──────────────┬──────────────┬───────────────┬───────────────────────────┤
│                    [🛡️ Gateway](apiGateways/Ocelot.APIGateway/README.md)                    │
│                             (8010)                               │
├──────────────┬──────────────┬───────────────┬───────────────────────────┤
│   [Basket](services/basket/README.md) │  [Catalog](services/catalog/README.md) │  [Discount](services/discount/README.md) │      [Ordering](services/ordering/README.md)      │
│     API      │     API      │     API    │        API                  │
│    (8081)    │    (8080)   │   (8082)   │       (8083)               │
├──────────────┼──────────────┼────────────┼───────────────────────────────┤
│    Redis     │  MongoDB    │ PostgreSQL│      SQL Server             │
│   Cache     │  Database   │ Database  │      Database               │
├──────────────┴──────────────┴────────────┴───────────────────────────┤
│                    [MassTransit + RabbitMQ](infrastructure/EventBus.Messages/README.md)              │
│                    (Event-driven communication)                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Services

| Service | Port | Database | Documentation |
|---------|------|----------|---------------|
| [Gateway](apiGateways/Ocelot.APIGateway/README.md) | 8010 | - | Ocelot API Gateway |
| [Basket](services/basket/README.md) | 8081 | Redis | Shopping cart management |
| [Catalog](services/catalog/README.md) | 8080 | MongoDB | Product catalog |
| [Discount](services/discount/README.md) | 8082 | PostgreSQL | Discount/coupon management |
| [Ordering](services/ordering/README.md) | 8083 | SQL Server | Order processing |

## Infrastructure

| Component | Documentation | Purpose |
|-----------|---------------|---------|
| [Common.Logging](infrastructure/Common.Logging/README.md) | Serilog | Structured logging |
| [EventBus.Messages](infrastructure/EventBus.Messages/README.md) | RabbitMQ | Shared message contracts |

## Run with Docker Compose

```bash
cd services/docker-compose
docker-compose up -d
```

## Key Dependencies

- **ASP.NET Core** 9.0 (Basket, Catalog, Discount), 10.0 (Ordering)
- **MongoDB Driver** — Catalog service persistence
- **StackExchange.Redis** — Basket caching
- **Entity Framework Core** — Ordering persistence
- **Dapper** — Discount data access
- **MassTransit** — Event bus (Basket → Ordering)
- **gRPC** — Discount service communication
- **MediatR** — CQRS pattern
- **FluentValidation** — Input validation
- **Serilog** — Structured logging

---

See [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) for detailed architecture documentation.