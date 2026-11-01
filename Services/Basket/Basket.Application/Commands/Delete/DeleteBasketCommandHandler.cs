using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commands.Delete
{
    public class DeleteBasketCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public DeleteBasketCommand(string userName)
        {
            UserName = userName;
        }
    }
    public class DeleteBasketCommandHandler(IBasketRepository basketRepository) : IRequestHandler<DeleteBasketCommand, bool>
    {
        public async Task<bool> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            await basketRepository.DeleteBasketAsync(request.UserName);
            return true;
        }
    }
}
