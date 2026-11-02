using System.Security.Claims;
using Basket.Application.Commands.Create;
using Basket.Application.Commands.Delete;
using Basket.Application.Queries.Basket;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Authorize]
    public class BasketController(IMediator mediator) : ApiController
    {
        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCartResponse?>> GetBasketByUserName(string userName, CancellationToken cancellationToken)
        {
            if (!IsCurrentUser(userName)) return Forbid();
            var result = await mediator.Send(new GetBasketByUserNameQuery(userName), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartResponse?>> CreateBasket([FromBody] CreateBasketCommand request, CancellationToken cancellationToken)
        {
            if (!IsCurrentUser(request.UserName)) return Forbid();
            return Ok(await mediator.Send(request, cancellationToken));
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult<Boolean>> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            if (!IsCurrentUser(userName)) return Forbid();
            return Ok(await mediator.Send(new DeleteBasketCommand(userName), cancellationToken));
        }

        private bool IsCurrentUser(string userName) =>
            string.Equals(User.FindFirstValue(ClaimTypes.Name), userName, StringComparison.OrdinalIgnoreCase);
    }
}