# Basket Service

Manages shopping cart operations with Redis caching.

## Overview

| Property | Value |
|-----------|-------|
| Port | 8081 |
| Database | Redis (StackExchange) |
| Framework | ASP.NET Core 9.0 |

## Project Structure

```
services/basket/
├── Basket.API/              # Entry point, controllers
├── Basket.Application/      # Commands, queries, handlers, DTOs
├── Basket.Core/              # Entities, repository interfaces
└── Basket.Infrastructure/     # Redis repository implementation
```

## Key Dependencies

- **StackExchange.Redis** — Cart caching
- **MassTransit + RabbitMQ** — Event publishing (BasketCheckoutEvent)
- **gRPC** — Discount service client
- **MediatR** — CQRS pattern
- **AutoMapper** — Object mapping
- **Swashbuckle** — API documentation

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/basket/{userName}` | Get cart by username |
| POST | `/basket` | Create/update cart |
| PUT | `/basket` | Update cart items |
| DELETE | `/basket/{userName}` | Delete cart |

## Communication

- **gRPC → Discount Service**: Fetches discount amounts for products
- **RabbitMQ → Ordering**: Publishes `BasketCheckoutEvent` on checkout

---

See [README.md](../../README.md) for full project documentation.