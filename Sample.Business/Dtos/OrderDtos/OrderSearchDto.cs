namespace Sample.Business.Dtos.OrderDtos;

public class OrderSearchDto {
    public IEnumerable<OrderDto> Orders { get; set; } = null!;
    public PaginationDto Page { get; set; } = null!;
}