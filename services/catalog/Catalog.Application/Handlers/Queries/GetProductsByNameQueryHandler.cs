using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.Queries
{
    public class GetProductsByNameQueryHandler : IRequestHandler<GetProductsByNameQuery, IList<ProductResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductsByNameQueryHandler> _logger;
        public GetProductsByNameQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<GetProductsByNameQueryHandler> logger
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting products by name {ProductName}", request.Name);
            var products = await _productRepository.GetProductsByNameAsync(request.Name);
            var productResponse = _mapper.Map<IList<ProductResponseDto>>(products.ToList());
            return productResponse;
        }
    }
}