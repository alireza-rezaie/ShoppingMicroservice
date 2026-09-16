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
    public class BrandRepository(ICatalogContext CatalogContex) : IBrandRepository
    {
        public async Task<IEnumerable<ProductBrand>> GetProductBrands()
        {
            return await CatalogContex.Brands.Find(t=>true).ToListAsync();
        }
    }
}
