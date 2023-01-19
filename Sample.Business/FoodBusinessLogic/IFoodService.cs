using Sample.Common.Dtos.FoodDtos;

namespace Sample.Business.FoodBusinessLogic
{
    public interface IFoodService
    {
        Task<IEnumerable<FoodDto>> GetAllFoodAsync();
        Task<string> AddFoodAsync(FoodAddDto foodDetails);
        Task<FoodDto> GetByIdAsync(long id);
        Task<string> UpdateFoodAsync(FoodDto foodDetails);
        Task<string> DeleteFoodAsync(long id);
    }
}
