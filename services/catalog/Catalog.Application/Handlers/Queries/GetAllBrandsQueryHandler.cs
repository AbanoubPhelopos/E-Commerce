using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        public GetAllBrandsQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands =  await _brandRepository.GetBrandsAsync();
            var brandResponse = _mapper.Map<IList<ProductBrand>,IList<BrandResponseDto>>(brands.ToList());
            return brandResponse;
        }
    }
}