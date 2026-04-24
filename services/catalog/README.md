# Catalog Service

Manages product catalog with brands, types, and products stored in MongoDB.

## Overview

| Property | Value |
|-----------|-------|
| Port | 8080 |
| Database | MongoDB |
| Framework | ASP.NET Core 9.0 |

## Project Structure

```
services/catalog/
├── Catalog.API/              # Entry point, controllers
├── Catalog.Application/      # Commands, queries, handlers, DTOs
├── Catalog.Core/              # Entities, specs, repository interfaces
└── Catalog.Infrastructure/    # MongoDB repositories, seed data
```

## Key Dependencies

- **MongoDB.Driver** — NoSQL database access
- **AspNetCore.HealthChecks.MongoDb** — Health monitoring
- **MediatR** — CQRS pattern
- **AutoMapper** — Object mapping
- **Swashbuckle** — API documentation

## Entities

- **Product** — Name, Description, Price, PictureUrl, Brand, Type, Stock
- **ProductBrand** — Name, Description
- **ProductType** — Name, Description

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/catalog/products` | List all products (paginated) |
| GET | `/catalog/products/{id}` | Get product by ID |
| GET | `/catalog/products/name/{name}` | Search products by name |
| GET | `/catalog/products/brand/{name}` | Get products by brand |
| GET | `/catalog/brands` | List all brands |
| GET | `/catalog/types` | List all types |

## Seed Data

The service seeds initial brands and types on startup via:
- `BrandContextSeed`
- `TypeContextSeed`
- `ProductContextSeed`

---

See [README.md](../../README.md) for full project documentation.