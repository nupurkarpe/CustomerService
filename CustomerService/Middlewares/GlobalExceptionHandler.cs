using CustomerService.Helper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace CustomerService.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message, details) = exception switch
            {
                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),

                ArgumentException =>
                    (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),

                InvalidOperationException =>
                    (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),

                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound, "Not Found", exception.Message),
                _ =>
                    (StatusCodes.Status500InternalServerError, "Internal Server Error", exception.Message)
            };



            context.Response.StatusCode = statusCode;

            var response = ApiResponse.Error(
                message: message,
               code: statusCode.ToString(),
                details: exception.Message
            );

            var responseBody = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(responseBody, cancellationToken);

            return true;
        }
    }
}
