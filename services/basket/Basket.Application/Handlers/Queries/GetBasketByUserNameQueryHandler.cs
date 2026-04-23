using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Handlers.Queries
{
    public class GetBasketByUserNameQueryHandler : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBasketByUserNameQueryHandler> _logger;

        public GetBasketByUserNameQueryHandler(IBasketRepository basketService, IMapper mapper, ILogger<GetBasketByUserNameQueryHandler> logger)
        {
            _basketService = basketService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Querying basket for user {UserName}", request.UserName);
            var basket = await _basketService.GetBasketAsync(request.UserName);
            var response = _mapper.Map<ShoppingCartResponse>(basket);
            return response;
        }
    }
}