using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Common
{
    public class CommonSpecsParams
    {
        private int _PageSize = 10;
        private const int _MaxPageSize = 70;
        public int PageSize { 
            get => _PageSize;
            set => _PageSize = value > _MaxPageSize ? _MaxPageSize : value ;
        }
        public int PageIndex { get; set; } = 1;
        public string Sort { get; set; }
        public string Search { get; set; }
    }
}
