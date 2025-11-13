using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastracture.Data
{
    public static class ProductContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<Product> productcollection)
        {
            var existingProducts = await productcollection.Find(_ => true).AnyAsync();
            if (existingProducts)
                return;
            var filePath = Path.Combine("Data", "SeedData", "products.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Seed file not found: {filePath}");
                return;
            }
            var productData = await File.ReadAllTextAsync(filePath);
            var products = JsonSerializer.Deserialize<List<Product>>(productData);
            if (products?.Any() is true)
            {
                await productcollection.InsertManyAsync(products);
            }
        }
    }
}