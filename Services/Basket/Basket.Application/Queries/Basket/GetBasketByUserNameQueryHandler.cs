using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Queries.Basket
{
    public class GetBasketByUserNameQuery(string userName) : IRequest<ShoppingCartResponse>
    {
        public string userName { get; set; } = userName;
    }
    public class GetBasketByUserNameQueryHandler(IBasketRepository basketRepository,IMapper mapper) : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {
        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasketByUserNameAsync(request.userName);
            if (basket == null) return new ShoppingCartResponse(request.userName);
            return mapper.Map<ShoppingCartResponse>(basket);
        }
    }
}
