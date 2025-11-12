using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetProductsByBrandNameQuery : IRequest<IList<ProductResponseDto>>
    {   
        public string BrandName { get; set; }
        public GetProductsByBrandNameQuery(string brandName)
        {
            BrandName = brandName;
        }
    }
}