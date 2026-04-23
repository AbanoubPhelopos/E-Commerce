using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<GetAllBrandsQueryHandler> _logger;
        public GetAllBrandsQueryHandler(IBrandRepository brandRepository, IMapper mapper, ILogger<GetAllBrandsQueryHandler> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all brands");
            var brands =  await _brandRepository.GetBrandsAsync();
            var brandResponse = _mapper.Map<IList<BrandResponseDto>>(brands.ToList());
            return brandResponse;
        }
    }
}