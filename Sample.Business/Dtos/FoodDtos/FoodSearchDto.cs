namespace Sample.Business.Dtos.FoodDtos;

public class FoodSearchDto {
    public IEnumerable<FoodDto> Foods { get; set; } = null!;
    public PaginationDto Page { get; set; } = null!;
}