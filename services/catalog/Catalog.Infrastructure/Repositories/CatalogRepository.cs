using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastracture.Data.Context;
using MongoDB.Driver;

namespace Catalog.Infrastracture.Repositories
{
    public class CatalogRepository : IProductRepository, IBrandRepository, ITypeRepository
    {
        private ICatalogContext _context { get; set; } 

        public CatalogRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }
        
        public async Task<bool> DeleteProductAsync(string id)
        {
            var deleteResult = await _context.Products.DeleteOneAsync(p => p.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        
        public async Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams specParams)
        {
            var filter = Builders<Product>.Filter.Empty;
            if (!string.IsNullOrEmpty(specParams.Search))
            {
                filter = Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(specParams.Search, "i"));
            }
            if (!string.IsNullOrEmpty(specParams.BrandId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Brand.Id, specParams.BrandId);
            }
            if (!string.IsNullOrEmpty(specParams.TypeId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Type.Id, specParams.TypeId);
            }

            var totalItems = await _context.Products.CountDocumentsAsync(filter);
            var data = await DataFilter(specParams, filter);     
            return new Pagination<Product>(specParams.PageIndex, specParams.PageSize, (int)totalItems, data);       
        }
        
        public async Task<Product> GetProductByIdAsync(string id) 
            => await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        
        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName)
            => await _context.Products.Find(p => p.Brand.Name == brandName).ToListAsync();
        
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string productName)
        => await _context.Products.Find(p => p.Name == productName).ToListAsync();
        
        public async Task<IEnumerable<Product>> GetProductsByTypeAsync(string typeName)
        => await _context.Products.Find(p => p.Type.Name == typeName).ToListAsync();

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        
        public async Task<IEnumerable<ProductBrand>> GetBrandsAsync()
        => await _context.Brands.Find(b => true).ToListAsync();
        
        public async Task<IEnumerable<ProductType>> GetTypesAsync()
        => await _context.Types.Find(t => true).ToListAsync();

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams specParams,FilterDefinition<Product> filter)
        {
            var sortDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
                {
                    case "priceasc":
                        sortDefinition = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "pricedesc":
                        sortDefinition = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
                        break;
                }
            }
            return await _context.Products.Find(filter)
                .Sort(sortDefinition)
                .Skip((specParams.PageIndex - 1) * specParams.PageSize)
                .Limit(specParams.PageSize)
                .ToListAsync();
        } 
    }
}