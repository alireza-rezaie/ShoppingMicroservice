using Basket.Application.Commands.Create;
using Basket.Application.Commands.Delete;
using Basket.Application.Queries.Basket;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    public class BasketController(IMediator mediator) : ApiController
    {
        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCartResponse?>> GetBasketByUserName(string userName, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetBasketByUserNameQuery(userName), cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ShoppingCartResponse?>> CreateBasket([FromBody] CreateBasketCommand request,CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(request, cancellationToken));
        }
        [HttpDelete("{userName}")]
        public async Task<ActionResult<Boolean>> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new DeleteBasketCommand(userName),cancellationToken));
        }
    }
}
