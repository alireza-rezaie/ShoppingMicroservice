using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName);

        Task<IEnumerable<Product>> GetProductsByBrandIdAsync(string brandId);
        Task<IEnumerable<Product>> GetProductsByTypeIdAsync(string typeId);
        Task<IEnumerable<Product>> GetProductsByTypeAsync(string type);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string Id);
        Task<bool> DeleteProductAsync(Product product);




    }
}
