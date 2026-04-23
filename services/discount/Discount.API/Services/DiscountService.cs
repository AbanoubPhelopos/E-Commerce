using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.API.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DiscountService> _logger;
        public DiscountService(IMediator mediator, ILogger<DiscountService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Getting discount for product {ProductName}", request.ProductName);
            var query = new GetDiscountQuery(request.ProductName);
            var couponModel = await _mediator.Send(query);
            return couponModel;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Creating discount for product {ProductName} with amount {Amount}", request.Coupon.ProductName, request.Coupon.Amount);
            var command = new CreateDiscountCommand(request.Coupon.ProductName, request.Coupon.Description, request.Coupon.Amount);
            var couponModel = await _mediator.Send(command);
            return couponModel;
        }
        
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Updating discount for product {ProductName} with id {Id}", request.Coupon.ProductName, request.Coupon.Id);
            var command = new UpdateDiscountCommand(request.Coupon.Id, request.Coupon.ProductName, request.Coupon.Description, request.Coupon.Amount);
            var couponModel = await _mediator.Send(command);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Deleting discount for product {ProductName}", request.ProductName);
            var command = new DeleteDiscountCommand(request.ProductName);
            var isDeleted = await _mediator.Send(command);
            _logger.LogInformation("Discount deletion for product {ProductName} result: {IsDeleted}", request.ProductName, isDeleted);
            return new DeleteDiscountResponse { Success = isDeleted };
        }
    }
}