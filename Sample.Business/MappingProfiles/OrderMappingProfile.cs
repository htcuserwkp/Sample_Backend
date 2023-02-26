using AutoMapper;
using Sample.Business.Dtos.OrderDtos;
using Sample.DataAccess.Entities;

namespace Sample.Business.MappingProfiles;

public class OrderMappingProfile : Profile {
    public OrderMappingProfile() {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ReverseMap();

        CreateMap<OrderItem, OrderItemDto>().ReverseMap();

        CreateMap<OrderAddDto, Order>();
    }
}