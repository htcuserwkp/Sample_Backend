using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Text.Json;
using FluentValidation;
using Sample.Common.Helpers.Exceptions;

namespace Sample.API.Extensions;

public static class ExceptionMiddlewareExtensions {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory, IHostEnvironment env) {

        app.UseStatusCodePagesWithReExecute("/errors/{0}");

        app.UseExceptionHandler(appError => {
            appError.Run(async httpContext => {
                var logger = loggerFactory.CreateLogger("ConfigureBuildInExceptionHandler");

                httpContext.Response.ContentType = "application/json";

                var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
                var contextRequest = httpContext.Features.Get<IHttpRequestFeature>()!;

                dynamic baseException = ((ExceptionHandlerFeature)contextFeature!).Error;

                var serializerOptionForCamelCase = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                if (baseException != null && baseException!.ToString().Contains("CustomException")) {
                    var customException = (CustomException)baseException!;

                    logger.LogError($"An exception triggered: {customException.CustomMessage}", contextRequest.Method,
                        contextRequest.Path);

                    httpContext.Response.StatusCode = (int)customException.HttpStatusCode;

                    await httpContext.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            new ErrorResponse() {
                                StatusCode = httpContext.Response.StatusCode,
                                Message = customException.CustomMessage
                            },
                            serializerOptionForCamelCase
                        ));
                }
                else if (contextFeature.Error is ValidationException validationException) {

                    logger.LogError($"A validation error occurred: {contextFeature.Error.Message}",
                        contextRequest.Method, contextRequest.Path);

                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        
                        await httpContext.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            new {
                                httpContext.Response.StatusCode,
                                Message = "Bad Request",
                                Errors = validationException.Errors
                                    .Select(e => $"{e.PropertyName} {e.ErrorMessage.Split("' ")[1]}")
                                    .ToArray()
                            },
                            serializerOptionForCamelCase
                        ));
                }
                else {
                    logger.LogError($"An unhandled exception occurred: {contextFeature.Error.Message}",
                        contextRequest.Method, contextRequest.Path);

                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    await httpContext.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            new ErrorResponse() {
                                StatusCode = httpContext.Response.StatusCode,
                                Message = env.IsDevelopment()
                                    ? contextFeature.Error.Message
                                    : "Internal Server Error"
                            },
                            serializerOptionForCamelCase
                        ));
                }
            });
        });
    }
}