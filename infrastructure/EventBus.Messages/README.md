# EventBus.Messages

Shared message contracts for RabbitMQ event communication between services.

## Overview

| Property | Value |
|-----------|-------|
| .NET | 9.0 |
| Type | Class Library (no external dependencies) |

## Contents

### Events

- **BaseIntegrationEvent** — Base class for all integration events
- **BasketCheckoutEvent** — Published when basket is checked out

### Constants

- **EventBusConstants** — Queue and exchange names

## Usage

```
Basket (publisher)
    └── BasketCheckoutEvent → RabbitMQ (basket-checkout-queue)
                                ↓
Ordering (consumer)
    └── Handles event → Creates order
```

Reference this library in services that need to publish or consume events:

```csharp
// Publishing
await _publishEndpoint.Publish(new BasketCheckoutEvent { ... });

// Consuming
public class BasketCheckoutEventConsumer : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // Handle checkout
    }
}
```

---

See [README.md](../../README.md) for full project documentation.