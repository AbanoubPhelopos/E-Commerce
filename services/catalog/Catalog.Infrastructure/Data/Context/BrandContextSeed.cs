using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastracture.Data.Context
{
    public static class BrandContextSeed
    {
        public static async Task SeedAsync(IMongoCollection<ProductBrand> brandCollection)
        {
            var existingBrands = await brandCollection.Find(_ => true).AnyAsync();
            if (existingBrands)
                return;
            var filePath = Path.Combine("Data", "SeedData", "brands.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Seed file not found: {filePath}");
                return;
            }
            var brandData = await File.ReadAllTextAsync(filePath);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
            if (brands?.Any() is true)
            {
                await brandCollection.InsertManyAsync(brands);
            }
        }
    }
}