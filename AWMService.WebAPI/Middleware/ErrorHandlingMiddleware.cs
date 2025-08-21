using System.Net;
using System.Text.Json;
using AWMService.Application.Exceptions;

namespace AWMService.WebAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception has occurred.");

            var response = context.Response;
            response.ContentType = "application/json";

            switch (exception)
            {
                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await response.WriteAsync(JsonSerializer.Serialize(new { errors = validationException.Errors }));
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await response.WriteAsync(JsonSerializer.Serialize(new { error = "An internal server error has occurred." }));
                    break;
            }
        }
    }
}
