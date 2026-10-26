using Catalog.Application.Commands.Products;
using Catalog.Application.Queries.Brands;
using Catalog.Application.Queries.Products;
using Catalog.Application.Queries.Types;
using Catalog.Application.Responses;
using Catalog.Core.CatalogSpecs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    public class CatalogController(IMediator mediator) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts([FromQuery] GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(request, cancellationToken));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id,CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetProductByIdQuery(id), cancellationToken)); 
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByName(string name, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetProductsByNameQuery(name), cancellationToken));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllBrands(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetAllProductBrandsQuery(), cancellationToken));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAllTypes(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetAllProductTypesQuery(), cancellationToken));
        }
        [HttpGet("{brandName}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductByBrandName(string brandName, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetProductsByBrandQuery(brandName), cancellationToken));
        }
        [HttpGet("{typeName}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductByTypeName(string typeName, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetProductsByTypeQuery(typeName), cancellationToken));
        }
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Createproduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(command,cancellationToken));
        }
        [HttpPut]
        public async Task<ActionResult<Boolean>> UpdateProduct([FromBody] UpdateProductCommand command,CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(command, cancellationToken));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new DeleteProductCommand(id), cancellationToken));
        }


    }
}
