using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrederRepository(OrderContext context) : RepositoryBase<Order>(context), IOrderRepository
{
    private OrderContext _context = context;
    
    public async Task<IEnumerable<Order>> GetOrdersByCustomerNameAsync(string userName,CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders.Where(o => o.UserName == userName).ToListAsync(cancellationToken);
        return orders;
    }
}