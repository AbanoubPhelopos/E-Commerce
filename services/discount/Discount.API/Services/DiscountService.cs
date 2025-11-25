using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;
        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var couponModel = await _mediator.Send(query);
            return couponModel;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var command = new CreateDiscountCommand(request.Coupon.ProductName, request.Coupon.Description, request.Coupon.Amount);
            var couponModel = await _mediator.Send(command);
            return couponModel;
        }
        
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var command = new UpdateDiscountCommand(request.Coupon.Id, request.Coupon.ProductName, request.Coupon.Description, request.Coupon.Amount);
            var couponModel = await _mediator.Send(command);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand(request.ProductName);
            var isDeleted = await _mediator.Send(command);
            return new DeleteDiscountResponse { Success = isDeleted };
        }
    }
}