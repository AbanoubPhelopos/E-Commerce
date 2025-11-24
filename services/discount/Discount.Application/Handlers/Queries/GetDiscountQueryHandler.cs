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

namespace Discount.Application.Handlers.Queries
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;

        public GetDiscountQueryHandler(IMapper mapper, IDiscountRepository discountRepository)
        {
            _mapper = mapper;
            _discountRepository = discountRepository;
        }

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var Coupons = await _discountRepository.GetDiscountAsync(request.ProductName);
            if(Coupons == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            return _mapper.Map<CouponModel>(Coupons);
        }
    }
}