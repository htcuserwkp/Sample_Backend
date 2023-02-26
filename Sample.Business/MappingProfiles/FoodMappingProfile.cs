using AutoMapper;
using Sample.Business.Dtos.FoodDtos;
using Sample.DataAccess.Entities;

namespace Sample.Business.MappingProfiles;

public class FoodMappingProfile : Profile {
    public FoodMappingProfile() {
        CreateMap<Food, FoodDto>().ReverseMap();
        CreateMap<FoodAddDto, Food>();
    }
}