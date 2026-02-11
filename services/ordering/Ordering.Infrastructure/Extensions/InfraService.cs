using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure.Extensions;

public static class InfraService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"),
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
        
        services.AddScoped(typeof(IRepository<>),typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrederRepository>();
        
        
        
        return services;
    }
}