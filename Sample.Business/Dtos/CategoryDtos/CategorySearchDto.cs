namespace Sample.Business.Dtos.CategoryDtos;

public class CategorySearchDto {
    public IEnumerable<CategoryDto> Categories { get; set; } = null!;
    public PaginationDto Page { get; set; } = null!;
}