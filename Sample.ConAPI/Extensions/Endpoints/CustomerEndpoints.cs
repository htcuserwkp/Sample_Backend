using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sample.DataAccess;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

namespace Sample.API.Extensions.Endpoints;

public static class CustomerEndpoints
{
    //TODO: Organize to comply solution architecture
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/Customer").WithTags(nameof(Customer));

        group.MapGet("/", async (IUnitOfWork db) => await db.CustomerRepo.GetAllAsync())
        .WithName("GetAllCustomers")
        .WithOpenApi();

        group.MapGet("/{id:long}", async Task<Results<Ok<Customer>, NotFound>> (long id, IUnitOfWork db) => await db.CustomerRepo.GetByIdAsync(id)
                is { } model
                ? TypedResults.Ok(model)
                : TypedResults.NotFound())
        .WithName("GetCustomerById")
        .WithOpenApi();

        group.MapPut("/{id:long}", async Task<Results<NotFound, NoContent>> (long id, Customer customer, IUnitOfWork db) =>
        {
            var foundModel = await db.CustomerRepo.GetByIdAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            await db.CustomerRepo.UpdateAsync(customer);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateCustomer")
        .WithOpenApi();

        group.MapPost("/", async (Customer customer, IUnitOfWork db) =>
        {
            await db.CustomerRepo.AddAsync(customer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/v1/Customer/{customer.Id}", customer);
        })
        .WithName("CreateCustomer")
        .WithOpenApi();

        group.MapDelete("/{id:long}", async Task<Results<Ok<Customer>, NotFound>> (long id, IUnitOfWork db) =>
        {
            if (await db.CustomerRepo.GetByIdAsync(id) is not { } customer) return TypedResults.NotFound();
            await db.CustomerRepo.DeleteAsync(customer);
            await db.SaveChangesAsync();
            return TypedResults.Ok(customer);
        })
        .WithName("DeleteCustomer")
        .WithOpenApi();
    }
}
