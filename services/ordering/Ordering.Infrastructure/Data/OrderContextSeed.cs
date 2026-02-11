using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public async Task SeeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            await orderContext.Orders.AddRangeAsync(GetOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seeded Orders to the database.");
        }
    }

    public static IEnumerable<Order> GetOrders()
    {
        return new List<Order>
        {
            new()
            {
                FirstName = "Abanoub",
                LastName = "Saweris",
                UserName = "Abanoub Saweris",
                EmailAddress = "abanoub.saweris@gmail.com",
                AddressLine = "Cairo, Egypt",
                Country = "Egypt",
                State = "Cairo",
                ZipCode = "12345",
                CardName = "Visa",
                CardNumber = "1234567890123456",
                Expiration = "12/25",
                CVV = "123",
                PaymentMethod = 1,
                TotalPrice = 100,
                OrderName = "Order 1"
            }
        };
    }
}