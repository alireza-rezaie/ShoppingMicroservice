using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketByUserNameAsync(string userName);
        Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart shoppingCart);
        Task DeleteBasketAsync(string userName);

    }
}
