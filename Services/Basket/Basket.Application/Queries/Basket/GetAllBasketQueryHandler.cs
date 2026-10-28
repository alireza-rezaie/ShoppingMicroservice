using Basket.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Queries.Basket
{
    public class GetAllBasketQuery : IRequest<ShoppingCartResponse>
    {
    }
    public class GetAllBasketQueryHandler : IRequestHandler<GetAllBasketQuery, ShoppingCartResponse>
    {
        public Task<ShoppingCartResponse> Handle(GetAllBasketQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
