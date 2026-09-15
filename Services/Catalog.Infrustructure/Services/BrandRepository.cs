using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrustructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrustructure.Services
{
    public class BrandRepository(ICatalogContext CatalogContex) : IBrandRepository
    {
        public Task<IEnumerable<ProductBrand>> GetProductBrands()
        {
            throw new NotImplementedException();
        }
    }
}
