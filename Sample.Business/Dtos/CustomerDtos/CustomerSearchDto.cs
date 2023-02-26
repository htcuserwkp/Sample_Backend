namespace Sample.Business.Dtos.CustomerDtos;

public class CustomerSearchDto {
    public IEnumerable<CustomerDto> Customers { get; set; } = null!;
    public PaginationDto Page { get; set; } = null!;
}