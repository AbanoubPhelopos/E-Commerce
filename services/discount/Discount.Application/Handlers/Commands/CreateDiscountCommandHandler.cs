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
    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<CreateDiscountCommandHandler> _logger;
        public CreateDiscountCommandHandler(IMapper mapper, IDiscountRepository discountRepository, ILogger<CreateDiscountCommandHandler> logger)
        {
            _mapper = mapper;
            _discountRepository = discountRepository;
            _logger = logger;
        }
        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating discount for product {ProductName}", request.ProductName);
            var coupon = _mapper.Map<Coupon>(request);
            var isCreated = await _discountRepository.CreateDiscountAsync(coupon);
            _logger.LogInformation("Discount creation for product {ProductName} result: {IsCreated}", request.ProductName, isCreated);
            return isCreated? _mapper.Map<CouponModel>(coupon) : null!;
        }
    }
}