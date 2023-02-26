using Microsoft.AspNetCore.Http.HttpResults;
using Sample.Business.Dtos.CustomerDtos;
using Sample.Business.Services.CustomerBusinessLogic;
using Sample.Common.Helpers.Response;
using System.Text.Json;

namespace Sample.API.Extensions.Endpoints;

public static class CustomerEndpoints {
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes, ILoggerFactory loggerFactory) {

        var group = routes.MapGroup("/api/v1/customer").WithTags("Customer");

        var logger = loggerFactory.CreateLogger("CustomerEndpoints");

        group.MapGet("/all", async Task<Results<Ok<ResponseBody<IEnumerable<CustomerDto>>>, NotFound>> (ICustomerService customerService) => {

            logger.LogInformation("Get all Customers request received.");

            var response = new ResponseBody<IEnumerable<CustomerDto>> {
                Data = await customerService.GetAllCustomerAsync().ConfigureAwait(false),
                Message = "Customers retrieved successfully"
            };

            logger.LogInformation(response.Message);
            return TypedResults.Ok(response);
        })
        .WithName("GetAllCustomers")
        .WithOpenApi();


        group.MapGet("/{id:long}", async Task<Results<Ok<ResponseBody<CustomerDto>>, NotFound>> (long id, ICustomerService customerService) => {

            logger.LogInformation($"Get request received for Customer ID: {id}.");

            var response = new ResponseBody<CustomerDto> {
                Data = await customerService.GetByIdAsync(id).ConfigureAwait(false),
                Message = "Customer retrieved successfully"
            };

            logger.LogInformation(response.Message);
            return TypedResults.Ok(response);
        })
        .WithName("GetCustomerById")
        .WithOpenApi();


        group.MapPut("/update", async Task<Results<Ok<ResponseBody<string>>, NotFound, NoContent>> (CustomerDto customerDetails, ICustomerService customerService) => {

            logger.LogInformation($"Update request received for Customer: {JsonSerializer.Serialize(customerDetails)}.");

            var response = new ResponseBody<string> {
                Message = await customerService.UpdateCustomerAsync(customerDetails).ConfigureAwait(false)
            };

            logger.LogInformation(response.Message);
            return TypedResults.Ok(response);
        })
        .WithName("UpdateCustomer")
        .WithOpenApi();


        group.MapPost("/add", async Task<Results<Ok<ResponseBody<string>>, NotFound, NoContent>> (CustomerAddDto customerDetails, ICustomerService customerService) => {

            logger.LogInformation($"Add request received for Customer: {JsonSerializer.Serialize(customerDetails)}.");

            var response = new ResponseBody<string> {
                Message = await customerService.AddCustomerAsync(customerDetails).ConfigureAwait(false)
            };

            logger.LogInformation(response.Message);
            return TypedResults.Ok(response);
        })
        .WithName("AddCustomer")
        .WithOpenApi();


        group.MapDelete("/{id:long}", async Task<Results<Ok<ResponseBody<string>>, NotFound, NoContent>> (long id, ICustomerService customerService) => {

            logger.LogInformation($"Delete request received for Customer ID: {id}.");

            var response = new ResponseBody<string> {
                Message = await customerService.DeleteCustomerAsync(id).ConfigureAwait(false)
            };

            logger.LogInformation(response.Message);
            return TypedResults.Ok(response);
        })
        .WithName("DeleteCustomer")
        .WithOpenApi();


        group.MapGet("/search", async Task<Results<Ok<ResponseBody<CustomerSearchDto>>, NotFound>> (ICustomerService customerService, string? keyword, int? skip, int? take, string? orderBy) => {
            
            logger.LogInformation("Search Customers request received.");

            var response = new ResponseBody<CustomerSearchDto> {
                Data = await customerService.SearchCustomersAsync(keyword, skip, take, orderBy).ConfigureAwait(false),
                Message = "Customers retrieved successfully"
            };

            logger.LogInformation(response.Message);
            return TypedResults.Ok(response);
        })
        .WithName("SearchCustomers")
        .WithOpenApi();
    }
}
