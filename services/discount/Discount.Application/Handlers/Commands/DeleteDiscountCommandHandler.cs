using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers.Commands
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository;
        public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await _discountRepository.DeleteDiscountAsync(request.ProductName);
            return isDeleted;
        }
    }
}