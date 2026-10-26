using AutoMapper;
using Catalog.Application.Queries.Types;
using Catalog.Application.Responses;
using Catalog.Core.CatalogSpecs;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries.Products
{
    public class GetAllProductsQuery : CatalogSpecParams,IRequest<Pagination<ProductResponse>>
    {

    }
    public class GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.GetProductsAsync(request);
            return mapper.Map<Pagination<ProductResponse>>(result);
        }
    }
}
