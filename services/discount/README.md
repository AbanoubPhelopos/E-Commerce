# Discount Service

Manages product discounts with gRPC interface for other services.

## Overview

| Property | Value |
|-----------|-------|
| Port | 8082 |
| Database | PostgreSQL |
| Framework | ASP.NET Core 9.0 |

## Project Structure

```
services/discount/
├── Discount.API/              # Entry point, gRPC service
├── Discount.Application/      # Commands, Protobuf definitions
├── Discount.Core/              # Entities
└── Discount.Infrastructure/    # PostgreSQL + Dapper repository
```

## Key Dependencies

- **gRPC.AspNetCore** — gRPC service host
- **Npgsql** — PostgreSQL access
- **Dapper** — Lightweight ORM
- **Protocol Buffers** — Contract definitions

## gRPC Service

```
DiscountProtoService
├── rpc GetDiscount (DiscountRequest) → DiscountResponse
```

### Request/Response

```protobuf
message DiscountRequest {
  string product_name = 1;
}

message DiscountResponse {
  string product_name = 1;
  int32 amount = 2;
}
```

## REST Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/discount/{productName}` | Get discount (REST fallback) |

---

See [README.md](../../README.md) for full project documentation.