using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DiscountRepository> _logger;
        public DiscountRepository(IConfiguration configuration, ILogger<DiscountRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            _logger.LogInformation("Creating discount for product {ProductName} with amount {Amount}", coupon.ProductName, coupon.Amount);
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            await using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var affected = connection.Execute(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });
            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            _logger.LogInformation("Deleting discount for product {ProductName}", productName);
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            await using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            return affected > 0;
        }

        public async Task<Coupon?> GetDiscountAsync(string productName)
        {
            _logger.LogInformation("Getting discount for product {ProductName}", productName);
            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            if (coupon == null)
            {
                _logger.LogWarning("Discount not found for product {ProductName}, returning default", productName);
                return new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Description"
                };
            }
            return coupon;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            _logger.LogInformation("Updating discount for product {ProductName} with id {Id}", coupon.ProductName, coupon.Id);
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            await using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });
            return affected > 0;
        }
    }
}