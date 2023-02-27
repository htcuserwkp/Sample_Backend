using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Sample.Business.Dtos;
using Sample.Business.Dtos.CategoryDtos;
using Sample.Business.Validators.CategoryValidators;
using Sample.Common.Helpers.Exceptions;
using Sample.Common.Helpers.PredicateBuilder;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

namespace Sample.Business.Services.CategoryBusinessLogic;

public class CategoryService : ICategoryService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoryAsync() {
        var categories = (await _unitOfWork.CategoryRepo.GetAllAsync()).ToList();

        if (!categories.Any()) {
            throw new CustomException {
                CustomMessage = "No Categories Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }
        var categoryList = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        _logger.LogDebug("Category details retrieved successfully.");
        return categoryList;
    }

    public async Task<string> AddCategoryAsync(CategoryAddDto categoryDetails) {
        string status;

        try {
            //TODO:Do validations

            var category = _mapper.Map<Category>(categoryDetails);

            await _unitOfWork.CategoryRepo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            status = "Category added successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Category failed to add";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }

        return status;
    }

    public async Task<CategoryDto> GetByIdAsync(long id) {
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(id);

        if (category is null) {
            throw new CustomException {
                CustomMessage = "No Category Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        var categoryInfo = _mapper.Map<CategoryDto>(category);
        _logger.LogDebug("Category details retrieved successfully.");

        return categoryInfo;
    }

    public async Task<string> UpdateCategoryAsync(CategoryDto categoryDetails) {
        string status;
        try {
            //validate details
            var validator = new CategoryUpdateValidator();
            await validator.ValidateAndThrowAsync(categoryDetails);

            var category = await _unitOfWork.CategoryRepo.GetByIdAsync(categoryDetails.Id);

            if (category is null) {
                throw new CustomException {
                    CustomMessage = "Category Not Found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            // Map category details to category entity
            _mapper.Map(categoryDetails, category);

            await _unitOfWork.CategoryRepo.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            status = "Category updated successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Category failed to update";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }

        return status;
    }

    public async Task<string> DeleteCategoryAsync(long id) {
        string status;
        try {
            //check availability
            if (!await _unitOfWork.CategoryRepo.IsActive(id)) {
                throw new CustomException {
                    CustomMessage = "Category not found!",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            await _unitOfWork.CategoryRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            status = "Category deleted successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Category deletion failed";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }
        return status;
    }

    public async Task<CategorySearchDto> SearchCategoriesAsync(string? keyword, int? skip, int? take, string? orderBy) {
        var categoryPredicate = PredicateBuilder.True<Category>();

        //filter by keyword
        if (!string.IsNullOrEmpty(keyword)) {
            categoryPredicate = SearchExpressionFilter(categoryPredicate, keyword);
        }

        //TODO: order by

        var categories = await _unitOfWork.CategoryRepo.GetAsync(predicate: categoryPredicate, skip: skip??0, take: take??10, orderBy: null);
        return new CategorySearchDto {
            Categories = _mapper.Map<IEnumerable<CategoryDto>>(categories),
            Page = new PaginationDto() {
                CurrentCount = categories.Count(),
                TotalCount = await _unitOfWork.CategoryRepo.GetCountAsync(categoryPredicate)
            }
        };
    }

    #region Private Methods
    private static Expression<Func<Category, bool>> SearchExpressionFilter(Expression<Func<Category, bool>> categoryPredicate, string keyword) {
        return categoryPredicate.And(c => (c.Name.Contains(keyword) ||
                                       c.Description.Contains(keyword)));
    }

    //private IEnumerable<CategoryDto> GetCategoriesList(IEnumerable<Category> categories) {
    //    return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    //}

    //private CategoryDto GetCategoryDetails(Category category) {
    //    return _mapper.Map<CategoryDto>(category);
    //}
    #endregion
}