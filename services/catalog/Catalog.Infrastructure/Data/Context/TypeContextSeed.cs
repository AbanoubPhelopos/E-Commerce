using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastracture.Data.Context
{
    public static class TypeContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<ProductType> typeCollection)
        {
            var existingTypes = await typeCollection.Find(_ => true).AnyAsync();
            if (existingTypes)
                return;

            var filePath = Path.Combine("Data", "SeedData", "types.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Seed file not found: {filePath}");
                return;
            }

            var typeData = await File.ReadAllTextAsync(filePath);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
            if (types?.Any() is true)
            {
                await typeCollection.InsertManyAsync(types);
            }
        }
    }
}