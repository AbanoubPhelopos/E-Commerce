using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Handlers.Commands
{
    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
    {
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<UpdateDiscountCommandHandler> _logger;
        public UpdateDiscountCommandHandler(IMapper mapper, IDiscountRepository discountRepository, ILogger<UpdateDiscountCommandHandler> logger)
        {
            _mapper = mapper;
            _discountRepository = discountRepository;
            _logger = logger;
        }
        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating discount for product {ProductName} with id {Id}", request.ProductName, request.Id);
            var coupon = _mapper.Map<Coupon>(request);
            var isUpdated = await _discountRepository.UpdateDiscountAsync(coupon);
            _logger.LogInformation("Discount update for product {ProductName} result: {IsUpdated}", request.ProductName, isUpdated);
            return isUpdated ? _mapper.Map<CouponModel>(coupon) : null!;
        }
    }
}