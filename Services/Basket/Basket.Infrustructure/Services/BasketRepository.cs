using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Infrustructure.Services
{
    public class BasketRepository(IDistributedCache redis) : IBasketRepository
    {
        public async Task<ShoppingCart?> GetBasketByUserNameAsync(string userName)
        {
            var basket = await redis.GetStringAsync(userName);
            return string.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket) ;
        }

        public async Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart shoppingCart)
        {
             await redis.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));
             return await GetBasketByUserNameAsync(shoppingCart.UserName);
        }

        public async Task DeleteBasketAsync(string userName)
        {
            await redis.RemoveAsync(userName);
        }
    }
}
