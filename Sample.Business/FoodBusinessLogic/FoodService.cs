using Sample.Common.Dtos.FoodDtos;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

namespace Sample.Business.FoodBusinessLogic;

public class FoodService : IFoodService
{
    private readonly IUnitOfWork _unitOfWork;

    public FoodService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<FoodDto>> GetAllFoodAsync()
    {
        var foods = (await _unitOfWork.FoodRepo.GetAllAsync()).ToList();

        if (!foods.Any()) return Enumerable.Empty<FoodDto>();
        var foodList = GetFoodsList(foods);
        return foodList;
    }

    public async Task<string> AddFoodAsync(FoodAddDto foodDetails)
    {
        string status;

        try
        {
            Food food = new()
            {
                Name = foodDetails.Name,
                Description = foodDetails.Description,
                Quantity = foodDetails.Quantity,
                Price = foodDetails.Price
            };

            await _unitOfWork.FoodRepo.AddAsync(food);
            await _unitOfWork.SaveChangesAsync();

            status = "Product created successfully";
        }
        catch
        {
            status = "Product created successfully";
        }

        return status;
    }

    #region Private Methods
    private static IEnumerable<FoodDto> GetFoodsList(IEnumerable<Food> foods)
    {
        return foods.Select(GetFoodDetails).ToList();
    }

    private static FoodDto GetFoodDetails(Food food)
    {
        FoodDto foodDetails = new()
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            Quantity = food.Quantity
        };
        return foodDetails;
    }
    #endregion
}