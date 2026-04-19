using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.Commands
{
    public class DeleteShoppingCartCommandHandler : IRequestHandler<DeleteShoppingCartCommand,Unit>
    {
        public readonly IBasketRepository _basketRepository;
        public DeleteShoppingCartCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Unit> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteBasketAsync(request.UserName);
            return Unit.Value;
        }
    }
}