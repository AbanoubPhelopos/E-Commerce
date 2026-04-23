using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
         private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateProductCommandHandler> _logger;
        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<CreateProductCommandHandler> logger
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating product {ProductName}", request.Name);
            var product = _mapper.Map<Product>(request);
            var newProduct = await _productRepository.CreateProductAsync(product);
            var productResponse = _mapper.Map<ProductResponseDto>(newProduct);
            _logger.LogInformation("Product {ProductName} created successfully with id {ProductId}", productResponse.Name, productResponse.Id);
            return productResponse;
        }
    }
}