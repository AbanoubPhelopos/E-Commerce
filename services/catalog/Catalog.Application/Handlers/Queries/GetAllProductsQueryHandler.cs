using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponseDto>>
    {
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;
    public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetAllProductsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

        public async Task<Pagination<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all products with page {PageIndex}, size {PageSize}", request.SpecParams.PageIndex, request.SpecParams.PageSize);
            var products = await _productRepository.GetProductsAsync(request.SpecParams);
            var productResponse = _mapper.Map<Pagination<ProductResponseDto>>(products);
            return productResponse;
        }
    }
}