using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams specParams);
        Task<Product> GetProductByIdAsync(string id);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName);
        Task<IEnumerable<Product>> GetProductsByTypeAsync(string typeName);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);
    }
}