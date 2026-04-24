# Common.Logging

Shared Serilog logging configuration for all services.

## Overview

| Property | Value |
|-----------|-------|
| .NET | 10.0 |
| Type | Class Library |

## Dependencies

- **Serilog.AspNetCore** — ASP.NET Core integration
- **Serilog.Sinks.Console** — Console output
- **Serilog.Sinks.Elasticsearch** — Elasticsearch sink (optional)

## Usage

Reference in any service project and configure in `Program.cs`:

```csharp
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
```

## Features

- Structured logging
- Console output for development
- Elasticsearch sink for production
- Request correlation IDs

---

See [README.md](../../README.md) for full project documentation.