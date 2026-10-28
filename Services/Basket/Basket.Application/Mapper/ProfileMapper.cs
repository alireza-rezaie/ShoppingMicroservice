using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<ShoppingCartItem, ShoppingCartItemResponse>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();

        }
    }
}
