using Discount.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Core.Repositories
{
    public interface IDiscountRepository
    {
        Task<Boolean> CreateDiscount(Coupon coupon);
        Task<Coupon> GetDiscountById(string productId);
        Task<Coupon> GetDiscountByName(string productName);
        Task<Boolean> UpdateDiscount(Coupon coupon);
        Task<Boolean> DeleteDiscount(string productId);
        Task<Boolean> DeleteDiscountByProductName(string productName);

    }
}
