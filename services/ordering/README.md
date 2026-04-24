# Ordering Service

Manages order processing with RabbitMQ event consumption.

## Overview

| Property | Value |
|-----------|-------|
| Port | 8083 |
| Database | SQL Server |
| Framework | ASP.NET Core 10.0 |

## Project Structure

```
services/ordering/
├── Ordering.Api/              # Entry point, controllers
├── Ordering.Application/      # Commands, queries, validators
├── Ordering.Core/              # Entities, interfaces, specs
└── Ordering.Infrastructure/    # EF Core + SQL Server repositories
```

## Key Dependencies

- **Entity Framework Core** — ORM
- **Microsoft.EntityFrameworkCore.SqlServer** — SQL Server provider
- **MassTransit + RabbitMQ** — Event consumption (BasketCheckoutEvent)
- **MediatR** — CQRS pattern
- **FluentValidation** — Input validation
- **AutoMapper** — Object mapping
- **Swashbuckle** — API documentation

## Entities

- **Order** — UserId, OrderDate, ShipToAddress, CardInfo, Items, Status
- **OrderItem** — ProductId, ProductName, UnitPrice, Quantity, PictureUrl

## Event Handlers

| Event | Source | Action |
|-------|--------|--------|
| BasketCheckoutEvent | Basket (RabbitMQ) | Creates order |

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/orders` | List user orders |
| GET | `/orders/{orderId}` | Get order by ID |
| POST | `/orders` | Create order |

## Order Status

- Pending → Paid → Shipped → Delivered

---

See [README.md](../../README.md) for full project documentation.