using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            await using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var affected = connection.Execute(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });
            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            await using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            return affected > 0;
        }

        public async Task<Coupon?> GetDiscountAsync(string productName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            if (coupon == null)
            {
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
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            await using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });
            return affected > 0;
        }
    }
}