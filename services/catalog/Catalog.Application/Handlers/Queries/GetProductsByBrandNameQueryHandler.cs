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

namespace Catalog.Application.Handlers.Queries
{
    public class GetProductsByBrandNameQueryHandler : IRequestHandler<GetProductsByBrandNameQuery, IList<ProductResponseDto>>
    {
         private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public GetProductsByBrandNameQueryHandler(
            IProductRepository productRepository,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductsByBrandNameQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsByBrandAsync(request.BrandName);
            var productResponse = _mapper.Map<IList<Product>, IList<ProductResponseDto>>(products.ToList());
            return productResponse;
        }
    }
}