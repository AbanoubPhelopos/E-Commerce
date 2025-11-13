using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastracture.Data.Context
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; } 

        public IMongoCollection<ProductBrand> Brands { get; }

        public IMongoCollection<ProductType> Types { get; }

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            
            Brands = database.GetCollection<ProductBrand>(configuration["DatabaseSettings:BrandsCollectionName"]);
            
            Types = database.GetCollection<ProductType>(configuration["DatabaseSettings:TypesCollectionName"]);

            Products = database.GetCollection<Product>(configuration["DatabaseSettings:ProductsCollectionName"]);

            _ = BrandContextSeed.SeedAsync(Brands);
            _ = TypeContextSeed.SeedDataAsync(Types);
            _ = ProductContextSeed.SeedDataAsync(Products);
        }
    }
}