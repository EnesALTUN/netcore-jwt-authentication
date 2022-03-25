using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Dtos;
using SharedLibrary.Exceptions;
using System.Net;
using System.Text.Json;

namespace SharedLibrary.Middlewares
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async (context) =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature is not null)
                    {
                        var ex = errorFeature.Error;

                        ErrorDto? errorDto = (ex is CustomException)
                            ? new(ex.Message, true)
                            : new(ex.Message, false);

                        var response = Response<NoDataDto>.Fail(errorDto, (int)HttpStatusCode.InternalServerError);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}