using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.Commands
{
    public class DeleteShoppinngCartCommandHandler : IRequestHandler<DeleteShoppinngCartCommand,Unit>
    {
        public readonly IBasketRepository _basketRepository;
        public DeleteShoppinngCartCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Unit> Handle(DeleteShoppinngCartCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteBasketAsync(request.UserName);
            return Unit.Value;
        }
    }
}