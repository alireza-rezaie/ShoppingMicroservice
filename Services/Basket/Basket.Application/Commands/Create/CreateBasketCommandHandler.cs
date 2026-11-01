using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commands.Create
{
    public class CreateBasketCommand(string userName, List<ShoppingCartItem> items) : IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; } = userName;
        public List<ShoppingCartItem> Items { get; set; } = items;
    }
    public class CreateBasketCommandHandler(IBasketRepository basketRepository, IMapper mapper) : IRequestHandler<CreateBasketCommand, ShoppingCartResponse>
    {
        public async Task<ShoppingCartResponse> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ShoppingCart shoppingCart = mapper.Map<ShoppingCart>(request);
                await basketRepository.UpdateBasketAsync(shoppingCart);
                return mapper.Map<ShoppingCartResponse>(shoppingCart);
            }
            catch (Exception ex)
            {
                throw new Exception("در هنگام ایجاد یا به‌روزرسانی سبد خرید خطایی رخ داده است." + ex);
            }
        }
    }
}
