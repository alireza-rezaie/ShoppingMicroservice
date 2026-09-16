using AutoMapper;
using Catalog.Application.Queries.Types;
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
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductResponse>>
    {

    }
    public class GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
    {
        public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.GetProductsAsync();
            return mapper.Map<IEnumerable<ProductResponse>>(result);
        }
    }
}
