using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sample.Business.Dtos;
using Sample.Business.Dtos.CustomerDtos;
using Sample.Common.Helpers.Exceptions;
using Sample.Common.Helpers.PredicateBuilder;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

namespace Sample.Business.Services.CustomerBusinessLogic;

public class CustomerService : ICustomerService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CustomerService> _logger;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, ILogger<CustomerService> logger, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomerAsync() {
        var customers = (await _unitOfWork.CustomerRepo.GetAllAsync()).ToList();

        if (!customers.Any()) {
            throw new CustomException {
                CustomMessage = "No Customers Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }
        var customerList = _mapper.Map<IEnumerable<CustomerDto>>(customers);
        _logger.LogDebug("Customer details retrieved successfully.");
        return customerList;
    }

    public async Task<string> AddCustomerAsync(CustomerAddDto customerDetails) {
        string status;

        try {
            //TODO:Do validations

            var customer = _mapper.Map<Customer>(customerDetails);

            await _unitOfWork.CustomerRepo.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            status = "Customer added successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Customer failed to add";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }

        return status;
    }

    public async Task<CustomerDto> GetByIdAsync(long id) {
        var customer = await _unitOfWork.CustomerRepo.GetByIdAsync(id);

        if (customer is null) {
            throw new CustomException {
                CustomMessage = "No Customer Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        var customerInfo = _mapper.Map<CustomerDto>(customer);
        _logger.LogDebug("Customer details retrieved successfully.");

        return customerInfo;
    }

    public async Task<string> UpdateCustomerAsync(CustomerDto customerDetails) {
        string status;
        try {
            //TODO: validate details

            var customer = await _unitOfWork.CustomerRepo.GetByIdAsync(customerDetails.Id);

            if (customer is null) {
                throw new CustomException {
                    CustomMessage = "Customer Not Found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            // Map customer details to customer entity
            _mapper.Map(customerDetails, customer);

            await _unitOfWork.CustomerRepo.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            status = "Customer updated successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Customer failed to update";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }

        return status;
    }

    public async Task<string> DeleteCustomerAsync(long id) {
        string status;
        try {
            //check availability
            if (!await _unitOfWork.CustomerRepo.IsActive(id)) {
                throw new CustomException {
                    CustomMessage = "Customer not found!",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            await _unitOfWork.CustomerRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            status = "Customer deleted successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Customer deletion failed";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }
        return status;
    }

    public async Task<CustomerSearchDto> SearchCustomersAsync(string? keyword, int? skip, int? take, string? orderBy) {
        var customerPredicate = PredicateBuilder.True<Customer>();

        //filter by keyword
        if (!string.IsNullOrEmpty(keyword)) {
            customerPredicate = SearchExpressionFilter(customerPredicate, keyword.ToLower().Trim());
        }

        //TODO: order by

        var customers = await _unitOfWork.CustomerRepo.GetAsync(predicate: customerPredicate, skip: skip ?? 0, take: take ?? 10, orderBy: null);
        return new CustomerSearchDto {
            Customers = _mapper.Map<IEnumerable<CustomerDto>>(customers),
            Page = new PaginationDto() {
                CurrentCount = customers.Count(),
                TotalCount = await _unitOfWork.CustomerRepo.GetCountAsync(customerPredicate)
            }
        };
    }

    #region Private Methods
    private static Expression<Func<Customer, bool>> SearchExpressionFilter(Expression<Func<Customer, bool>> customerPredicate, string keyword) {
        return customerPredicate.And(c => (c.Name.ToLower().Trim().Contains(keyword) ||
                                       c.Email.ToLower().Trim().Contains(keyword) ||
                                       c.Phone.Trim().Contains(keyword)));
    }

    //private IEnumerable<CustomerDto> GetCustomersList(IEnumerable<Customer> customers) {
    //    return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    //}

    //private CustomerDto GetCustomerDetails(Customer customer) {
    //    return _mapper.Map<CustomerDto>(customer);
    //}
    #endregion
}