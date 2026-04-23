using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Handlers.Commands
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DeleteDiscountCommandHandler> _logger;
        public DeleteDiscountCommandHandler(IDiscountRepository discountRepository, ILogger<DeleteDiscountCommandHandler> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting discount for product {ProductName}", request.ProductName);
            var isDeleted = await _discountRepository.DeleteDiscountAsync(request.ProductName);
            _logger.LogInformation("Discount deletion for product {ProductName} result: {IsDeleted}", request.ProductName, isDeleted);
            return isDeleted;
        }
    }
}