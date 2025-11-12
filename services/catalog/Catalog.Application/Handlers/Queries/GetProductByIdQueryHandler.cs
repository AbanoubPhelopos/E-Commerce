using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public GetProductByIdQueryHandler(
            IProductRepository productRepository,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id);
            var productResponse = _mapper.Map<ProductResponseDto>(product);
            return productResponse;
        }
    }
}