using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly ITypeRepository _typeRepository;
        public GetAllTypesQueryHandler(
            ITypeRepository typeRepository,
            IMapper mapper
        )
        {
            _typeRepository = typeRepository;
            _mapper = mapper;
        }
        public async Task<IList<TypesResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeRepository.GetTypesAsync();
            var typeResponse = _mapper.Map<IList<Core.Entities.ProductType>, IList<TypesResponseDto>>(types.ToList());
            return typeResponse;
        }
    }
}