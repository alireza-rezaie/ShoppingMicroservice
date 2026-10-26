using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.CatalogSpecs
{
    public class Pagination<T>(IReadOnlyList<T> data, int count, int pageIndex, int pageSize) where T : class 
    {
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> Data { get; set; } = data;
    }
}
