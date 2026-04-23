using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductByIdQueryHandler> _logger;
        public GetProductByIdQueryHandler(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<GetProductByIdQueryHandler> logger
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting product by id {ProductId}", request.Id);
            var product = await _productRepository.GetProductByIdAsync(request.Id);
            var productResponse = _mapper.Map<ProductResponseDto>(product);
            return productResponse;
        }
    }
}