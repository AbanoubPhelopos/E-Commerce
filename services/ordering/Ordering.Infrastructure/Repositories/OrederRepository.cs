using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrederRepository(OrderContext context, ILogger<OrederRepository> logger) : RepositoryBase<Order>(context, logger), IOrderRepository
{
    private OrderContext _context = context;
    private readonly ILogger<OrederRepository> _logger = logger;
    
    public async Task<IEnumerable<Order>> GetOrdersByCustomerNameAsync(string userName,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting orders for user {UserName}", userName);
        var orders = await _context.Orders.Where(o => o.UserName == userName).ToListAsync(cancellationToken);
        return orders;
    }
}