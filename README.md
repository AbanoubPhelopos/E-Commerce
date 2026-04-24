# E-Commerce Microservices

A distributed e-commerce platform built with ASP.NET microservices, following Clean Architecture principles.

## Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                    E-Commerce Solution                                │
├──────────────┬──────────────┬───────────────┬───────────────────────────┤
│   Basket   │  Catalog   │  Discount  │      Ordering          │
│   API     │    API     │    API    │        API            │
│  (Port    │  (Port     │  (Port    │      (Port           │
│   8081)   │   8080)    │   8082)   │       8083)         │
├──────────────┼──────────────┼────────────┼───────────────────────────┤
│  Redis     │  MongoDB    │ PostgreSQL │      SQL Server         │
│  Cache    │  Database   │ Database  │      Database          │
├──────────────┴──────────────┴────────────┴───────────────────────────┤
│                    MassTransit + RabbitMQ                          │
│                    (Event-driven communication)                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Services

| Service | Port | Technology | Purpose |
|---------|------|-----------|---------|
| [Basket](services/basket/README.md) | 8081 | ASP.NET Core, Redis, MassTransit, gRPC | Shopping cart management |
| [Catalog](services/catalog/README.md) | 8080 | ASP.NET Core, MongoDB | Product catalog (brands, types, products) |
| [Discount](services/discount/README.md) | 8082 | ASP.NET Core, PostgreSQL, gRPC | Discount/coupon management |
| [Ordering](services/ordering/README.md) | 8083 | ASP.NET Core, SQL Server, MassTransit | Order processing |

## Infrastructure

| Component | Technology | Purpose |
|-----------|-----------|---------|
| [Common.Logging](infrastructure/Common.Logging) | Serilog | Structured logging |
| [EventBus.Messages](infrastructure/EventBus.Messages) | Class Library | Shared message contracts |

## Run with Docker Compose

```bash
cd services/docker-compose
docker-compose up -d
```

## Key Dependencies

- **ASP.NET Core** 9.0 (Basket, Catalog, Discount), 10.0 (Ordering)
- **MongoDB Driver** - Catalog service persistence
- **StackExchange.Redis** - Basket caching
- **Entity Framework Core** - Ordering persistence
- **Dapper** - Discount data access
- **MassTransit** - Event bus (Basket → Ordering)
- **gRPC** - Discount service communication
- **MediatR** - CQRS pattern
- **FluentValidation** - Input validation
- **Serilog** - Structured logging

---

See [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) for detailed architecture documentation.