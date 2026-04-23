using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.Queries
{
    public class GetProductsByBrandNameQueryHandler : IRequestHandler<GetProductsByBrandNameQuery, IList<ProductResponseDto>>
    {
         private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductsByBrandNameQueryHandler> _logger;
        public GetProductsByBrandNameQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<GetProductsByBrandNameQueryHandler> logger
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductsByBrandNameQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting products by brand name {BrandName}", request.BrandName);
            var products = await _productRepository.GetProductsByBrandAsync(request.BrandName);
            var productResponse = _mapper.Map<IList<ProductResponseDto>>(products.ToList());
            return productResponse;
        }
    }
}