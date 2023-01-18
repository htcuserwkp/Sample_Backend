using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sample.DataAccess;
using Sample.DataAccess.Entities;

namespace Sample.API.Extensions.Endpoints;

public static class CategoryEndpoints
{
    //TODO: Organize to comply solution architecture
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Category").WithTags(nameof(Category));

        group.MapGet("/", async (SampleAppDbContext db) => await db.Categories.ToListAsync())
        .WithName("GetAllCategories")
        .WithOpenApi();

        group.MapGet("/{id:long}", async Task<Results<Ok<Category>, NotFound>> (long id, SampleAppDbContext db) => await db.Categories.FindAsync(id)
                is { } model
                ? TypedResults.Ok(model)
                : TypedResults.NotFound())
        .WithName("GetCategoryById")
        .WithOpenApi();

        group.MapPut("/{id:long}", async Task<Results<NotFound, NoContent>> (long id, Category category, SampleAppDbContext db) =>
        {
            var foundModel = await db.Categories.FindAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            db.Update(category);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateCategory")
        .WithOpenApi();

        group.MapPost("/", async (Category category, SampleAppDbContext db) =>
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Category/{category.Id}", category);
        })
        .WithName("CreateCategory")
        .WithOpenApi();

        group.MapDelete("/{id:long}", async Task<Results<Ok<Category>, NotFound>> (long id, SampleAppDbContext db) =>
        {
            if (await db.Categories.FindAsync(id) is not { } category) return TypedResults.NotFound();
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return TypedResults.Ok(category);

        })
        .WithName("DeleteCategory")
        .WithOpenApi();
    }
}
