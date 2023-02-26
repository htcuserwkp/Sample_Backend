using Sample.Business.Dtos.CustomerDtos;

namespace Sample.Business.Services.CustomerBusinessLogic;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<string> AddCustomerAsync(CustomerAddDto customerDetails);
    Task<CustomerDto> GetByIdAsync(long id);
    Task<string> UpdateCustomerAsync(CustomerDto customerDetails);
    Task<string> DeleteCustomerAsync(long id);
}