using Catalog.Application.Commands.Products;
using Catalog.Application.Queries.Brands;
using Catalog.Application.Queries.Products;
using Catalog.Application.Queries.Types;
using Catalog.Application.Responses;
using Catalog.Core.CatalogSpecs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    public class CatalogController(IMediator mediator) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts([FromQuery] GetAllProductsQuery request, CancellationToken cancellationToken)
            => Ok(await mediator.Send(request, cancellationToken));

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id, CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetProductByIdQuery(id), cancellationToken));

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByName(string name, CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetProductsByNameQuery(name), cancellationToken));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllBrands(CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetAllProductBrandsQuery(), cancellationToken));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAllTypes(CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetAllProductTypesQuery(), cancellationToken));

        [HttpGet("{brandName}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductByBrandName(string brandName, CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetProductsByBrandQuery(brandName), cancellationToken));

        [HttpGet("{typeName}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductByTypeName(string typeName, CancellationToken cancellationToken)
            => Ok(await mediator.Send(new GetProductsByTypeQuery(typeName), cancellationToken));

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Createproduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
            => Ok(await mediator.Send(command, cancellationToken));

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Boolean>> UpdateProduct([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
            => Ok(await mediator.Send(command, cancellationToken));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id, CancellationToken cancellationToken)
            => Ok(await mediator.Send(new DeleteProductCommand(id), cancellationToken));
    }
}