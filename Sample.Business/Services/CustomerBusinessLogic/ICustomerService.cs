using Sample.Business.Dtos.CustomerDtos;

namespace Sample.Business.Services.CustomerBusinessLogic;

public interface ICustomerService {

    Task<IEnumerable<CustomerDto>> GetAllCustomerAsync();
    Task<string> AddCustomerAsync(CustomerAddDto customerDetails);
    Task<CustomerDto> GetByIdAsync(long id);
    Task<string> UpdateCustomerAsync(CustomerDto customerDetails);
    Task<string> DeleteCustomerAsync(long id);
    Task<CustomerSearchDto> SearchCustomersAsync(string? keyword,
        int? skip,
        int? take,
        string? orderBy);
}