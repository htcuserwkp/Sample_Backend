using Sample.Business.Dtos.FoodDtos;

namespace Sample.Business.Services.FoodBusinessLogic;

public interface IFoodService {

    Task<IEnumerable<FoodDto>> GetAllFoodAsync();
    Task<string> AddFoodAsync(FoodAddDto foodDetails);
    Task<FoodDto> GetByIdAsync(long id);
    Task<string> UpdateFoodAsync(FoodDto foodDetails);
    Task<string> DeleteFoodAsync(long id);
    Task<FoodSearchDto> SearchFoodsAsync(string? keyword = null,
                                            int skip = 0,
                                            int take = 10,
                                            string? orderBy = null,
                                            long categoryId = 0);
}