using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountprotoService;
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountprotoService)
        {
            _discountprotoService = discountprotoService;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            return await _discountprotoService.GetDiscountAsync(request);
        }
    }
}