using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.Commands
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;

        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
        }

        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Items)
            {
                var coupon =  await _discountGrpcService.GetDiscount(item.ProductName);
                if(coupon is not null)
                    item.Price -= coupon.Amount;
            }
            var basket = new ShoppingCart
            {
                UserName = request.UserName,
                Items = request.Items
            };
            var createdBasket = await _basketRepository.UpdateBasketAsync(basket);
            var response = _mapper.Map<ShoppingCartResponse>(createdBasket);
            return response;
        }
    }
}