using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Logging;

namespace Basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountprotoService;
        private readonly ILogger<DiscountGrpcService> _logger;
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountprotoService, ILogger<DiscountGrpcService> logger)
        {
            _discountprotoService = discountprotoService;
            _logger = logger;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            _logger.LogInformation("Fetching discount via gRPC for product {ProductName}", productName);
            var request = new GetDiscountRequest { ProductName = productName };
            var response = await _discountprotoService.GetDiscountAsync(request);
            _logger.LogInformation("Discount fetched for product {ProductName} with amount {Amount}", productName, response.Amount);
            return response;
        }
    }
}