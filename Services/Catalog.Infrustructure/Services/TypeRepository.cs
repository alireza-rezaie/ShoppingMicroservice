using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrustructure.Services
{
    public class TypeRepository : ITypeRepository
    {
        public Task<IEnumerable<ProductType>> GetProductTypes()
        {
            throw new NotImplementedException();
        }
    }
}
