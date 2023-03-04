using Microsoft.AspNetCore.Mvc;
using Sample.Common.Helpers.Exceptions;

namespace Sample.API.Extensions;

public static class ValidationErrorExtensions {
    public static void ConfigureValidationErrors(this IServiceCollection services) {

        services.Configure<ApiBehaviorOptions>(options => {
            options.InvalidModelStateResponseFactory = actionContext => {
                var errors = actionContext.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value?.Errors!)
                    .Select(x => x.ErrorMessage)
                    .ToArray();

                var errorResponse = new ValidationErrorResponse {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });
    }
}