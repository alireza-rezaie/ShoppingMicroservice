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
        public static string GetBasketKey(string userName) => $"basket:{userName}";
        public async Task<ShoppingCart?> GetBasketByUserNameAsync(string userName)
        {
            var basket = await redis.GetStringAsync(GetBasketKey(userName));
            return string.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket) ;
        }

        public async Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart shoppingCart)
        {
            if (string.IsNullOrEmpty(shoppingCart.UserName))
                throw new Exception("نام کاربری نمیتواند خالی باشد");

            var option = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            };

            var json = JsonConvert.SerializeObject(shoppingCart);
            await redis.SetStringAsync(GetBasketKey(shoppingCart.UserName), json, option);
            return await GetBasketByUserNameAsync(shoppingCart.UserName);
        }

        public async Task DeleteBasketAsync(string userName)
        {
            await redis.RemoveAsync(GetBasketKey(userName));
        }
    }
}
