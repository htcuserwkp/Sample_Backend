namespace Sample.Business.Dtos.FoodDtos;

public class FoodSearchDto {
    public IEnumerable<FoodDto> Products { get; set; } = null!;
    public PaginationDto Page { get; set; } = null!;
}