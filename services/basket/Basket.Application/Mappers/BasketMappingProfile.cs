using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;


namespace Basket.Application.Mappers
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();

            CreateMap<ShoppingCartItems, ShoppingCartItemResponse>().ReverseMap();

            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();

            CreateMap<BasketCheckoutV2, BasketCheckoutEventV2>().ReverseMap();
        }
    }
}