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

namespace Discount.Application.Handlers.Commands
{
    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
    {
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;
        public UpdateDiscountCommandHandler(IMapper mapper, IDiscountRepository discountRepository)
        {
            _mapper = mapper;
            _discountRepository = discountRepository;
        }
        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Coupon>(request);
            var isUpdated = await _discountRepository.UpdateDiscountAsync(coupon);
            return isUpdated ? _mapper.Map<CouponModel>(coupon) : null!;
        }
    }
}