using AutoMapper;
using Microsoft.Extensions.Logging;
using Sample.Business.Dtos.CategoryDtos;
using Sample.Common.Helpers.Exceptions;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;
using System.Net;

namespace Sample.Business.Services.CategoryBusinessLogic;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = (await _unitOfWork.CategoryRepo.GetAllAsync()).ToList();

        if (!categories.Any())
        {
            throw new CustomException
            {
                CustomMessage = "No categories Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }
        var categoryList = GetCategoriesList(categories);
        _logger.LogDebug("Category details retrieved successfully.");
        return categoryList;
    }

    public async Task<string> AddCategoryAsync(CategoryAddDto categoryDetails)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryDto> GetByIdAsync(long id)
    {
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(id);

        if (category is null)
        {
            throw new CustomException
            {
                CustomMessage = "No category Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }
        var categoryInfo = GetCategoryDetails(category);
        _logger.LogDebug("Category details retrieved successfully.");
        return categoryInfo;
    }

    public async Task<string> UpdateCategoryAsync(CategoryDto categoryDetails)
    {
        throw new NotImplementedException();
    }

    public async Task<string> DeleteCategoryAsync(long id)
    {
        throw new NotImplementedException();
    }

    #region Private Methods
    private IEnumerable<CategoryDto> GetCategoriesList(IEnumerable<Category> categories)
    {
        return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);
    }
    //private static IEnumerable<CategoryDto> GetCategoriesList(IEnumerable<Category> categories)
    //{
    //    return categories.Select(GetCategoryDetails).ToList();
    //}

    private CategoryDto GetCategoryDetails(Category category)
    {
        return _mapper.Map<Category, CategoryDto>(category);
    }
    #endregion
}