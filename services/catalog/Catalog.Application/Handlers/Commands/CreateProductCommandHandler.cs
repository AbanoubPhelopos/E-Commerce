using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
         private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            var newProduct = await _productRepository.CreateProductAsync(product);
            var productResponse = _mapper.Map<ProductResponseDto>(newProduct);
            return productResponse;
        }
    }
}