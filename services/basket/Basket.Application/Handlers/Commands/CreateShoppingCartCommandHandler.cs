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
using Microsoft.Extensions.Logging;

namespace Basket.Application.Handlers.Commands
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateShoppingCartCommandHandler> _logger;

        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, IMapper mapper, ILogger<CreateShoppingCartCommandHandler> logger)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating shopping cart for user {UserName} with {ItemCount} items", request.UserName, request.Items.Count);
            foreach (var item in request.Items)
            {
                var coupon =  await _discountGrpcService.GetDiscount(item.ProductName);
                if(coupon is not null)
                {
                    _logger.LogInformation("Applying discount coupon for product {ProductName} with amount {Amount}", item.ProductName, coupon.Amount);
                    item.Price -= coupon.Amount;
                }
            }
            var basket = new ShoppingCart
            {
                UserName = request.UserName,
                Items = request.Items
            };
            var createdBasket = await _basketRepository.UpdateBasketAsync(basket);
            var response = _mapper.Map<ShoppingCartResponse>(createdBasket);
            _logger.LogInformation("Shopping cart created for user {UserName}", request.UserName);
            return response;
        }
    }
}