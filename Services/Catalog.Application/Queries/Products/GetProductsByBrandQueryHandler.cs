using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries.Products
{
    public class GetProductsByBrandQuery : IRequest<IEnumerable<ProductResponse>>
    {
        public string Brand { get; set; }
        public GetProductsByBrandQuery(string brand)
        {
            Brand = brand;
        }
    }
    public class GetProductsByBrandQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductsByBrandQuery, IEnumerable<ProductResponse>>
    {
        public async Task<IEnumerable<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.GetProductsByBrandAsync(request.Brand);
            if (result == null) throw new Exception($"Not Found Brand {request.Brand}");
            return mapper.Map<IEnumerable<ProductResponse>>(result);
        }
    }
}
