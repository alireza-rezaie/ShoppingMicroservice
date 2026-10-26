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
    public class TypeRepository(ICatalogContext catalogContext) : ITypeRepository
    {
        public async Task<IEnumerable<ProductType>> GetProductTypes()
        {
            return await catalogContext.Types.Find(t => true).ToListAsync();
        }
    }
}
