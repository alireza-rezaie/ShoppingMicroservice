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
    public class GetProductByIdQuery : IRequest<ProductResponse>
    {
        public string Id { get; set; }
        public GetProductByIdQuery(string id)
        {
            Id = id;
        }
    }
    public class GetProductByIdQueryHandler(IProductRepository productRepository ,IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.GetProductByIdAsync(request.Id);
            if (result == null) throw new Exception($"Not Found Product {request.Id}");
            return mapper.Map<ProductResponse>(result);
        }
    }
}
