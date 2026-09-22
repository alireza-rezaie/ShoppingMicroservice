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
    public class GetProductsByNameQuery : IRequest<ProductResponse>
    {
        public string Name { get; set; }
        public GetProductsByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class GetProductsByNameQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductsByNameQuery, ProductResponse>
    {
        public async Task<ProductResponse> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.GetProductByNameAsync(request.Name);
            if (result == null) throw new Exception($"Not Found Product {request.Name}");
            return mapper.Map<ProductResponse>(result);
        }
    }
}
