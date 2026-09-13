using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrustructure.Services
{
    public class ProductRepository : IProductRepository
    {
        public Task<bool> DeleteProductAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByBrandIdAsync(string brandId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByTypeAsync(string type)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByTypeIdAsync(string typeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
