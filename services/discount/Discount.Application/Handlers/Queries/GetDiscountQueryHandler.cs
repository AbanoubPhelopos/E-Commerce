using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Handlers.Queries
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<GetDiscountQueryHandler> _logger;

        public GetDiscountQueryHandler(IMapper mapper, IDiscountRepository discountRepository, ILogger<GetDiscountQueryHandler> logger)
        {
            _mapper = mapper;
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting discount for product {ProductName}", request.ProductName);
            var Coupons = await _discountRepository.GetDiscountAsync(request.ProductName);
            if(Coupons == null)
            {
                _logger.LogWarning("Discount not found for product {ProductName}", request.ProductName);
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            return _mapper.Map<CouponModel>(Coupons);
        }
    }
}