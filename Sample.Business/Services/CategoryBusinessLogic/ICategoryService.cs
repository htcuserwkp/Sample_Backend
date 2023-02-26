using Sample.Business.Dtos.CategoryDtos;

namespace Sample.Business.Services.CategoryBusinessLogic;

public interface ICategoryService {

    Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
    Task<string> AddCategoryAsync(CategoryAddDto categoryDetails);
    Task<CategoryDto> GetByIdAsync(long id);
    Task<string> UpdateCategoryAsync(CategoryDto categoryDetails);
    Task<string> DeleteCategoryAsync(long id);
    Task<CategorySearchDto> SearchCategoriesAsync(string? keyword,
        int? skip,
        int? take,
        string? orderBy);
}