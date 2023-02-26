using Sample.Business.Dtos.CustomerDtos;

namespace Sample.Business.Services.CustomerBusinessLogic;

public class CustomerService : ICustomerService
{
    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddCustomerAsync(CustomerAddDto customerDetails)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomerDto> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UpdateCustomerAsync(CustomerDto customerDetails)
    {
        throw new NotImplementedException();
    }

    public async Task<string> DeleteCustomerAsync(long id)
    {
        throw new NotImplementedException();
    }
}