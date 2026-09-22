using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrustructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrustructure.Services
{
    public class ProductRepository(ICatalogContext catalogContext) : IProductRepository
    {
        public async Task<Product> CreateProduct(Product product)
        {
            await catalogContext.Products.InsertOneAsync(product);
            return product;
            
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await catalogContext.Products.Find(t=>true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await catalogContext.Products.Find(t=>t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await catalogContext.Products.Find(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            return await catalogContext.Products.Find(x=>x.Brands.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName)
        {
            return await catalogContext.Products.Find(x=>x.Brands.Name == brandName).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandIdAsync(string brandId)
        {
            return await catalogContext.Products.Find(x => x.Brands.Id == brandId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByTypeAsync(string type)
        {
            return await catalogContext.Products.Find(x=> x.Types.Name == type).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByTypeIdAsync(string typeId)
        {
            return await catalogContext.Products.Find(x => x.Types.Id == typeId).ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var result = await catalogContext.Products.ReplaceOneAsync(x => x.Id == product.Id,product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var result = await catalogContext.Products.DeleteOneAsync(x => x.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            return await DeleteProductAsync(product.Id);
        }

    }
}
