using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sample.DataAccess;
using Sample.DataAccess.Entities;

namespace Sample.API.EndpointExtensions;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Customer").WithTags(nameof(Customer));

        group.MapGet("/", async (SampleAppDbContext db) => await db.Customers.ToListAsync())
        .WithName("GetAllCustomers")
        .WithOpenApi();

        group.MapGet("/{id:long}", async Task<Results<Ok<Customer>, NotFound>> (long id, SampleAppDbContext db) => await db.Customers.FindAsync(id)
                is { } model
                ? TypedResults.Ok(model)
                : TypedResults.NotFound())
        .WithName("GetCustomerById")
        .WithOpenApi();

        group.MapPut("/{id:long}", async Task<Results<NotFound, NoContent>> (long id, Customer customer, SampleAppDbContext db) =>
        {
            var foundModel = await db.Customers.FindAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            db.Update(customer);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateCustomer")
        .WithOpenApi();

        group.MapPost("/", async (Customer customer, SampleAppDbContext db) =>
        {
            db.Customers.Add(customer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Customer/{customer.Id}", customer);
        })
        .WithName("CreateCustomer")
        .WithOpenApi();

        group.MapDelete("/{id:long}", async Task<Results<Ok<Customer>, NotFound>> (long id, SampleAppDbContext db) =>
        {
            if (await db.Customers.FindAsync(id) is not { } customer) return TypedResults.NotFound();
            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return TypedResults.Ok(customer);

        })
        .WithName("DeleteCustomer")
        .WithOpenApi();
    }
}
