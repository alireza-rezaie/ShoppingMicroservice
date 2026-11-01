using AutoMapper.Configuration;
using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public ShoppingCartResponse()
        {
            
        }
        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }
        public Guid Guid { get; set; } 
        public string? UserName { get; set; } 
        public List<ShoppingCartItemResponse> Items { get; set; } = new();
        public decimal CalculateOrginalPrice()
        {
            return Items.Sum(x => x.Quantity * x.Price);
        }
    }
}
