using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "Abanoub Saweris";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedAtUtc = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "Abanoub Saweris";
                    break;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess,cancellationToken);
    }
}