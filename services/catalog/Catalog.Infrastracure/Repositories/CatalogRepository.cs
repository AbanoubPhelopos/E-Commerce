using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastracure.Data.Context;
using MongoDB.Driver;

namespace Catalog.Infrastracure.Repositories
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
        
        public async Task<IEnumerable<Product>> GetProductsAsync()
            => await _context.Products.Find(p => true).ToListAsync();
        
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
    }
}