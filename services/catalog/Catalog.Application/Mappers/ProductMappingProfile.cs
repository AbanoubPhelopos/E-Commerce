using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductBrand, BrandResponseDto>().ReverseMap();

            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Id.ToString() : null))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : null))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Type != null ? src.Type.Id.ToString() : null))
                .ReverseMap();
                
        }
    }
}