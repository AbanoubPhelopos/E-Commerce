using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Ordering.Api.Extensions;

public static class DbExtension
{
    public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContext}", typeof(TContext).Name);

            var retry = Policy.Handle<SqlException>()
                .WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, timeSpan, attempt, ctx) =>
                    {
                        logger.LogWarning(exception, "Retry {Attempt} after {TimeSpan}s", attempt, timeSpan.TotalSeconds);
                    }
                );

            retry.Execute(() => context.Database.Migrate());
            await seeder(context, services);
            logger.LogInformation("Database migration completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database");
            throw;
        }

        return host;
    }
}
