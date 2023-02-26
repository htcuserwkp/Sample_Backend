using AutoMapper;
using Sample.Business.Dtos.CategoryDtos;
using Sample.DataAccess.Entities;

namespace Sample.Business.MappingProfiles;

public class CategoryMappingProfile : Profile {
    public CategoryMappingProfile() => CreateMap<Category, CategoryDto>().ReverseMap();
}