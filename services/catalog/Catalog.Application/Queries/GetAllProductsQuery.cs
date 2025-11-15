using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllProductsQuery : IRequest<Pagination<ProductResponseDto>>
    {
        
    }
}