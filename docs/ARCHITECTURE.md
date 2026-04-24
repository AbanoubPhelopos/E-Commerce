# E-Commerce — Architecture Documentation

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Services](#services)
- [Infrastructure](#infrastructure)
- [Technology Stack](#technology-stack)
- [Communication Patterns](#communication-patterns)
- [Project Structure](#project-structure)
- [Run Locally](#run-locally)

---

## Architecture Overview

This is a **Microservices** e-commerce platform built with ASP.NET. Each service is a separate, deployable unit with its own database.

```
┌─────────────────────────────────────────────────────────────────────┐
│                 E-Commerce Solution                       │
├───────────────┬───────────────┬────────────┬────────────────┤
│    Basket    │   Catalog    │  Discount │    Ordering    │
│     API      │     API      │    API   │      API       │
│   (8081)     │   (8080)    │  (8082)  │    (8083)     │
├───────────────┼───────────────┼────────────┼────────────────┤
│    Redis      │   MongoDB    │ PostgreSQL│   SQL Server   │
│   Cache      │  Database    │ Database  │   Database     │
├───────────────┴───────────────┴────────────┴────────────────┤
│              MassTransit + RabbitMQ                        │
│            (async events between services)                 │
└─────────────────────────────────────────────────────────────┘
```

### Architecture Pattern

Clean Architecture / Layered Architecture within each service:

```
┌─────────────────────────────────────┐
│         API Layer (Entry Point)      │
│    Controllers / Minimal APIs        │
├─────────────────────────────────────┤
│       Application Layer              │
│  Commands, Queries, MediatR        │
├─────────────────────────────────────┤
│          Core Layer                 │
│   Domain Entities, Interfaces        │
├─────────────────────────────────────┤
│       Infrastructure Layer          │
│   DB, Cache, External Services     │
└─────────────────────────────────────┘
```

---

## Services

### Basket Service

| Aspect | Details |
|--------|---------|
| **Port** | 8081 |
| **Location** | `services/basket/` |
| **Database** | Redis (StackExchangeRedis) |
| **Purpose** | Shopping cart management — add items, update quantities, checkout |

#### Project Structure
- `Basket.API` — Web API, gRPC client, MassTransit
- `Basket.Application` — Commands, MediatR handlers
- `Basket.Core` — Domain entities, interfaces
- `Basket.Infrastructure` — Redis repository

#### Key Dependencies
- StackExchange.Redis — caching
- MassTransit + RabbitMQ — event publishing
- gRPC — communicate with Discount service

#### API Endpoints
- `GET /basket/{userId}` — Get cart
- `POST /basket` — Add item
- `PUT /basket` — Update cart
- `DELETE /basket/{userId}` — Clear cart

---

### Catalog Service

| Aspect | Details |
|--------|---------|
| **Port** | 8080 |
| **Location** | `services/catalog/` |
| **Database** | MongoDB |
| **Purpose** | Product catalog — brands, types, products |

#### Project Structure
- `Catalog.API` — Web API
- `Catalog.Application` — Commands, Queries, MediatR
- `Catalog.Core` — Entities, Repository interfaces
- `Catalog.Infrastructure` — MongoDB repositories, Seed data

#### Key Dependencies
- MongoDB.Driver — NoSQL database
- AspNetCore.HealthChecks.MongoDb — Health monitoring

#### API Endpoints
- `GET /catalog/products` — List products (with pagination, filtering)
- `GET /catalog/products/{id}` — Get product
- `GET /catalog/brands` — List brands
- `GET /catalog/types` — List types

---

### Discount Service

| Aspect | Details |
|--------|---------|
| **Port** | 8082 |
| **Location** | `services/discount/` |
| **Database** | PostgreSQL |
| **Purpose** | Discount/coupon management — provides gRPC service |

#### Project Structure
- `Discount.API` — gRPC service host
- `Discount.Application` — Commands, Protobuf
- `Discount.Core` — Entities
- `Discount.Infrastructure` — PostgreSQL + Dapper

#### Key Dependencies
- gRPC.AspNetCore — gRPC service
- Npgsql + Dapper — PostgreSQL access
- Protobuf — contract definition

#### gRPC Service
- `DiscountProtoService.GetDiscount(productName)` — returns discount amount

---

### Ordering Service

| Aspect | Details |
|--------|---------|
| **Port** | 8083 |
| **Location** | `services/ordering/` |
| **Database** | SQL Server (EF Core) |
| **Purpose** | Order processing — create orders, manage order entities |

#### Project Structure
- `Ordering.Api` — Web API, RabbitMQ consumer
- `Ordering.Application` — Commands, FluentValidation
- `Ordering.Core` — Entities, Interfaces
- `Ordering.Infrastructure` — EF Core + SQL Server |

#### Key Dependencies
- Entity Framework Core — ORM
- Microsoft.EntityFrameworkCore.SqlServer — SQL Server
- MassTransit + RabbitMQ — event consumption (Basket checkout)
- FluentValidation — input validation

#### Event Handlers
- Consumes `BasketCheckoutEvent` from RabbitMQ — creates order on checkout

---

## Infrastructure

### Common.Logging

| Aspect | Details |
|--------|---------|
| **Location** | `infrastructure/Common.Logging/` |
| **.NET** | 10.0 |
| **Purpose** | Shared Serilog configuration |

#### Dependencies
- Serilog.AspNetCore
- Serilog.Sinks.Console
- Serilog.Sinks.Elasticsearch (optional)

---

### EventBus.Messages

| Aspect | Details |
|--------|---------|
| **Location** | `infrastructure/EventBus.Messages/` |
| **.NET** | 9.0 |
| **Purpose** | Shared message contracts for RabbitMQ |

#### Contents
- `BaseIntegrationEvent` — base class for events
- `BasketCheckoutEvent` — published when basket is checked out
- `EventBusConstants` — queue/exchange names

---

## Technology Stack

| Category | Technology | Services |
|----------|------------|----------|
| **Runtime** | ASP.NET Core 9.0 | Basket, Catalog, Discount |
| **Runtime** | ASP.NET Core 10.0 | Ordering |
| **HTTP** | Minimal APIs / Controllers | All |
| **CQRS** | MediatR | All |
| **Validation** | FluentValidation | Ordering |
| **Database** | MongoDB | Catalog |
| **Database** | PostgreSQL + Dapper | Discount |
| **Database** | SQL Server + EF Core | Ordering |
| **Caching** | Redis (StackExchange) | Basket |
| **Messaging** | MassTransit + RabbitMQ | Basket, Ordering |
| **gRPC** | Grpc.Net | Discount |
| **Logging** | Serilog | All |

---

## Communication Patterns

### Synchronous (gRPC)

```
Basket → Discount (gRPC)
   Request discount for product
   Returns discount amount
```

### Asynchronous (MassTransit + RabbitMQ)

```
Basket → RabbitMQ (BasketCheckoutEvent)
   |
   ↓
Ordering (consumes event)
   Creates order
```

### Flow

```
1. User adds items to cart (Basket API)
2. Basket calculates total, requests discount via gRPC (Discount API)
3. User checks out
4. Basket publishes BasketCheckoutEvent to RabbitMQ
5. Ordering consumes event, creates order in SQL Server
```

---

## Project Structure

```
E-Commerce/
├── ECommerce.sln
├── README.md
│
├── services/
│   ├── basket/
│   │   ├── Basket.API/
│   │   ├── Basket.Application/
│   │   ├── Basket.Core/
│   │   └── Basket.Infrastructure/
│   │
│   ├── catalog/
│   │   ├── Catalog.API/
│   │   ├── Catalog.Application/
│   │   ├── Catalog.Core/
│   │   └── Catalog.Infrastructure/
│   │
│   ├── discount/
│   │   ├── Discount.API/
│   │   ├── Discount.Application/
│   │   ├── Discount.Core/
│   │   └── Discount.Infrastructure/
│   │
│   ├── ordering/
│   │   ├── Ordering.Api/
│   │   ├── Ordering.Application/
│   │   ├── Ordering.Core/
│   │   └── Ordering.Infrastructure/
│   │
│   └── docker-compose/
│       └── docker-compose.yml
│
├── infrastructure/
│   ├── Common.Logging/
│   └── EventBus.Messages/
│
└── docs/
    └── ARCHITECTURE.md
```

---

## Run Locally

### Prerequisites
- .NET 9.0 or 10.0 SDK
- Docker Desktop
- Redis, MongoDB, PostgreSQL, SQL Server (via Docker)

### Docker Compose

```bash
cd services/docker-compose
docker-compose up -d
```

### Individual Services

```bash
# Basket
dotnet run --project services/basket/Basket.API

# Catalog  
dotnet run --project services/catalog/Catalog.API

# Discount
dotnet run --project services/discount/Discount.API

# Ordering
dotnet run --project services/ordering/Ordering.Api
```

### Ports

| Service | Port |
|--------|------|
| Basket API | 8081 |
| Catalog API | 8080 |
| Discount API | 8082 |
| Ordering API | 8083 |
| RabbitMQ | 5672 (AMQP), 15672 (UI) |
| Elasticsearch | 9200 |
| Kibana | 5601 |

---

## Cross-cutting Concerns

- **API Versioning** — All services use `Asp.Versioning.Mvc`
- **Structured Logging** — via Common.Logging (Serilog)
- **CQRS** — via MediatR library
- **Health Checks** — `/health` endpoints per service