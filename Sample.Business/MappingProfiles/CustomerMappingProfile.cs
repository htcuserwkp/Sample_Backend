using AutoMapper;
using Sample.Business.Dtos.CustomerDtos;
using Sample.DataAccess.Entities;

namespace Sample.Business.MappingProfiles;

public class CustomerMappingProfile : Profile {
    public CustomerMappingProfile() => CreateMap<Customer, CustomerDto>().ReverseMap();
}