using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Handlers.Commands
{
    public class DeleteShoppingCartCommandHandler : IRequestHandler<DeleteShoppingCartCommand,Unit>
    {
        public readonly IBasketRepository _basketRepository;
        private readonly ILogger<DeleteShoppingCartCommandHandler> _logger;
        public DeleteShoppingCartCommandHandler(IBasketRepository basketRepository, ILogger<DeleteShoppingCartCommandHandler> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting shopping cart for user {UserName}", request.UserName);
            await _basketRepository.DeleteBasketAsync(request.UserName);
            _logger.LogInformation("Shopping cart deleted for user {UserName}", request.UserName);
            return Unit.Value;
        }
    }
}