using Microsoft.AspNetCore.Http.HttpResults;
using Sample.Business.Dtos.CategoryDtos;
using Sample.Business.Services.CategoryBusinessLogic;
using Sample.Common.Helpers.Response;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

namespace Sample.API.Extensions.Endpoints;

public static class CategoryEndpoints
{
    //TODO: Organize to comply solution architecture
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/category").WithTags(nameof(Category));

        group.MapGet("/get-all", async Task<Results<Ok<ResponseBody<IEnumerable<CategoryDto>>>, NotFound>> (ICategoryService categoryService) =>
        {
            var response = new ResponseBody<IEnumerable<CategoryDto>>
            {
                Data = await categoryService.GetAllAsync().ConfigureAwait(false),
                Message = "Categories retrieved successfully"
            };

            return TypedResults.Ok(response);
        })
        .WithName("GetAllCategories")
        .WithOpenApi();

        //group.MapGet("/{id:long}", async Task<Results<Ok<ResponseBody<CategoryDto>>, NotFound>> (long id, IUnitOfWork db) =>
        //{
        //    return await db.CategoryRepo.GetByIdAsync(id)
        //        is { } model
        //        ? TypedResults.Ok(model)
        //        : TypedResults.NotFound();
        //})
        //.WithName("GetCategoryById")
        //.WithOpenApi();

        group.MapPut("/{id:long}", async Task<Results<NotFound, NoContent>> (long id, Category category, IUnitOfWork db) =>
        {
            var foundModel = await db.CategoryRepo.GetByIdAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            await db.CategoryRepo.UpdateAsync(category);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateCategory")
        .WithOpenApi();

        group.MapPost("/", async (Category category, IUnitOfWork db) =>
        {
            await db.CategoryRepo.AddAsync(category);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Category/{category.Id}", category);
        })
        .WithName("CreateCategory")
        .WithOpenApi();

        group.MapDelete("/{id:long}", async Task<Results<Ok<Category>, NotFound>> (long id, IUnitOfWork db) =>
        {
            if (await db.CategoryRepo.GetByIdAsync(id) is not { } category) return TypedResults.NotFound();
            await db.CategoryRepo.DeleteAsync(id);
            await db.SaveChangesAsync();
            return TypedResults.Ok(category);

        })
        .WithName("DeleteCategory")
        .WithOpenApi();
    }
}
