using Catalog.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.CatalogSpecs
{
    public class CatalogSpecParams : CommonSpecsParams
    {
        public string BrandId { get; set; }
        public string TypeId { get; set; }

    }
}
