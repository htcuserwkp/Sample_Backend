using Sample.Business.Dtos.CategoryDtos;

namespace Sample.Business.Services.CategoryBusinessLogic;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<string> AddCategoryAsync(CategoryAddDto categoryDetails);
    Task<CategoryDto> GetByIdAsync(long id);
    Task<string> UpdateCategoryAsync(CategoryDto categoryDetails);
    Task<string> DeleteCategoryAsync(long id);
}