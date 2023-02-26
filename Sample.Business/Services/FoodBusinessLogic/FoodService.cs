using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sample.Business.Dtos.FoodDtos;
using Sample.Common.Helpers.Exceptions;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

namespace Sample.Business.Services.FoodBusinessLogic;

public class FoodService : IFoodService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FoodService> _logger;
    private readonly IMapper _mapper;

    public FoodService(IUnitOfWork unitOfWork, ILogger<FoodService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FoodDto>> GetAllFoodAsync()
    {
        var foods = (await _unitOfWork.FoodRepo.GetAllAsync()).ToList();

        if (!foods.Any())
        {
            throw new CustomException
            {
                CustomMessage = "No Foods Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }
        var foodList = _mapper.Map<IEnumerable<FoodDto>>(foods);
        _logger.LogDebug("Food details retrieved successfully.");
        return foodList;
    }

    public async Task<string> AddFoodAsync(FoodAddDto foodDetails)
    {
        string status;

        try
        {
            //TODO:Do validations

            //check category
            if (!await _unitOfWork.CategoryRepo.IsActive(foodDetails.CategoryId)) {
                throw new CustomException {
                    CustomMessage = "Selected Category Not Found!",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            var food = _mapper.Map<Food>(foodDetails);

            await _unitOfWork.FoodRepo.AddAsync(food);
            await _unitOfWork.SaveChangesAsync();

            status = "Food added successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) 
        {
            status = "Food failed to add";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }

        return status;
    }

    public async Task<FoodDto> GetByIdAsync(long id)
    {
        var food = await _unitOfWork.FoodRepo.GetByIdAsync(id);

        if (food is null)
        {
            throw new CustomException
            {
                CustomMessage = "No Food Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        var foodInfo = _mapper.Map<FoodDto>(food);
        _logger.LogDebug("Food details retrieved successfully.");

        return foodInfo;
    }

    public async Task<string> UpdateFoodAsync(FoodDto foodDetails)
    {
        string status;
        try 
        {
            //TODO: validate details

            var food = await _unitOfWork.FoodRepo.GetByIdAsync(foodDetails.Id);

            if (food is null) {
                throw new CustomException {
                    CustomMessage = "Food Not Found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            // Map food details to food entity
            _mapper.Map(foodDetails, food);

            await _unitOfWork.FoodRepo.UpdateAsync(food);
            await _unitOfWork.SaveChangesAsync();

            status = "Food updated successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) 
        {
            status = "Food failed to update";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }

        return status;
    }

    public async Task<string> DeleteFoodAsync(long id)
    {
        string status;
        try 
        {
            //check availability
            if (!await _unitOfWork.FoodRepo.IsActive(id)) {
                throw new CustomException {
                    CustomMessage = "Food not found!",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            await _unitOfWork.FoodRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            status = "Food deleted successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) 
        {
            status = "Food deletion failed";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }
        return status;
    }

    public async Task<FoodSearchDto> SearchAsync(string keyword, int skip = 0, int take = 10, string? orderBy = null, long categoryId = 0)
    {
        //TODO:
        throw new NotImplementedException();
    }

    #region Private Methods
    //private IEnumerable<FoodDto> GetFoodsList(IEnumerable<Food> foods)
    //{
    //    return _mapper.Map<IEnumerable<FoodDto>>(foods);
    //}

    //private FoodDto GetFoodDetails(Food food)
    //{
    //    return _mapper.Map<FoodDto>(food);
    //}
    #endregion
}