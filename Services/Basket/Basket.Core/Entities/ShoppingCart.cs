using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Entities
{
    public class ShoppingCart(string userName,string userId)
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = userName;
        public List<ShoppingCartItem> Items { get; set; } = new();
        public decimal CalculateOrginalPrice()
        {
            return Items.Sum(x => x.Quantity * x.Price);
        }
    }
}
