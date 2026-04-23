using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
         private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UpdateProductCommandHandler> _logger;
        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<UpdateProductCommandHandler> logger
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product {ProductId}", request.Id);
            var product = _mapper.Map<Product>(request);
            var isUpdated = await _productRepository.UpdateProductAsync(product);
            _logger.LogInformation("Product {ProductId} update result: {IsUpdated}", request.Id, isUpdated);
            return isUpdated;
        }
    }
}