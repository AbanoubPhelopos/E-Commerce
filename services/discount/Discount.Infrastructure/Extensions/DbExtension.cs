using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            try
            {
                ApplyMigrations(configuration);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<TContext>>();
                logger.LogError(ex, "An error occurred while migrating the database");
            }

            return host;
        }
        private static void ApplyMigrations(IConfiguration configuration)
        {
            var retry=5;
            while (retry-- > 0)
            {
                try
                {
                    using var connection = new NpgsqlConnection(
                        configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE Coupon(
                                            Id SERIAL PRIMARY KEY NOT NULL,
                                            ProductName VARCHAR(500) NOT NULL,
                                            Description TEXT,
                                            Amount INT)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Egypt Adidas Quick Force Indoor Badminton Shoes', 'Adidas Discount', 1500);";
                    command.ExecuteNonQuery();
                    break;
                }
                catch (NpgsqlException)
                {
                    if (retry == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(2000);
                }

            }
        }
    }
}