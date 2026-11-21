using Discount.Application.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Queries
{
    public class GetDiscountByNameQuery: IRequest<CouponModel>
    {
        public string ProductName { get; set; }
        public GetDiscountByNameQuery(string productName)
        {
            ProductName = productName;
        }
    }
    public class GetDiscountByNameQueryHandler : IRequestHandler<GetDiscountByNameQuery, CouponModel>
    {
        public Task<CouponModel> Handle(GetDiscountByNameQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
