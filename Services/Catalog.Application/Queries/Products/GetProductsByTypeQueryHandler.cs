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
    public class GetProductsByTypeQuery : IRequest<IEnumerable<ProductResponse>>
    {
        public string Type { get; set; }
        public GetProductsByTypeQuery(string type)
        {
            Type = type;
        }
    }
    public class GetProductsByTypeQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductsByTypeQuery, IEnumerable<ProductResponse>>
    {
        public async Task<IEnumerable<ProductResponse>> Handle(GetProductsByTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.GetProductsByTypeAsync(request.Type);
            if (result == null) throw new Exception($"Not Found Brand {request.Type}");
            return mapper.Map<IEnumerable<ProductResponse>>(result);
        }
    }
}
