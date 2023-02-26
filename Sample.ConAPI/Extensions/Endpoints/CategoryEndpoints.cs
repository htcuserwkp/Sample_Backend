using Microsoft.AspNetCore.Http.HttpResults;
using Sample.Business.Dtos.CategoryDtos;
using Sample.Business.Services.CategoryBusinessLogic;
using Sample.Common.Helpers.Response;

namespace Sample.API.Extensions.Endpoints;

public static class CategoryEndpoints {
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes) {

        var group = routes.MapGroup("/api/v1/category").WithTags("Category");


        group.MapGet("/all", async Task<Results<Ok<ResponseBody<IEnumerable<CategoryDto>>>, NotFound>> (ICategoryService categoryService) => {
                var response = new ResponseBody<IEnumerable<CategoryDto>> {
                    Data = await categoryService.GetAllCategoryAsync().ConfigureAwait(false),
                    Message = "Categories retrieved successfully"
                };

                return TypedResults.Ok(response);
            })
            .WithName("GetAllCategories")
            .WithOpenApi();


        group.MapGet("/{id:long}", async Task<Results<Ok<ResponseBody<CategoryDto>>, NotFound>> (long id, ICategoryService categoryService) => {
                var response = new ResponseBody<CategoryDto> {
                    Data = await categoryService.GetByIdAsync(id).ConfigureAwait(false),
                    Message = "Category retrieved successfully"
                };

                return TypedResults.Ok(response);
            })
            .WithName("GetCategoryById")
            .WithOpenApi();


        group.MapPut("/update", async Task<Results<Ok<ResponseBody<string>>, NotFound, NoContent>> (CategoryDto categoryDetails, ICategoryService categoryService) => {
                var response = new ResponseBody<string> {
                    Message = await categoryService.UpdateCategoryAsync(categoryDetails).ConfigureAwait(false)
                };

                return TypedResults.Ok(response);
            })
            .WithName("UpdateCategory")
            .WithOpenApi();


        group.MapPost("/add", async Task<Results<Ok<ResponseBody<string>>, NotFound, NoContent>> (CategoryAddDto categoryDetails, ICategoryService categoryService) => {
                var response = new ResponseBody<string> {
                    Message = await categoryService.AddCategoryAsync(categoryDetails).ConfigureAwait(false)
                };

                return TypedResults.Ok(response);
            })
            .WithName("AddCategory")
            .WithOpenApi();


        group.MapDelete("/{id:long}", async Task<Results<Ok<ResponseBody<string>>, NotFound, NoContent>> (long id, ICategoryService categoryService) => {
                var response = new ResponseBody<string> {
                    Message = await categoryService.DeleteCategoryAsync(id).ConfigureAwait(false)
                };

                return TypedResults.Ok(response);
            })
            .WithName("DeleteCategory")
            .WithOpenApi();


        group.MapGet("/search", async Task<Results<Ok<ResponseBody<CategorySearchDto>>, NotFound>> (ICategoryService categoryService, string? keyword, int? skip, int? take, string? orderBy) => {
                var response = new ResponseBody<CategorySearchDto> {
                    Data = await categoryService.SearchCategoriesAsync(keyword, skip, take, orderBy).ConfigureAwait(false),
                    Message = "Categories retrieved successfully"
                };

                return TypedResults.Ok(response);
            })
            .WithName("SearchCategories")
            .WithOpenApi();
    }
}
