using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AWMService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult GenerateProblemResponse(Error error)
        {
            ProblemDetails problemDetails;

            switch (error.Code)
            {
                case ErrorCode.NotFound:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Not Found",
                        Detail = error.Message ?? "No data found",
                        Status = 404
                    };
                    break;
                case ErrorCode.BadRequest:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Bad Request",
                        Detail = error.Message ?? "Invalid request",
                        Status = 400
                    };
                    break;
                case ErrorCode.Unauthorized:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Unauthorized",
                        Detail = error.Message ?? "Not authorized",
                        Status = 401
                    };
                    break;
                case ErrorCode.Forbidden:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Forbidden",
                        Detail = error.Message ?? "Access forbidden",
                        Status = 403
                    };
                    break;
                case ErrorCode.Conflict:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Conflict",
                        Detail = error.Message ?? "Data conflict",
                        Status = 409
                    };
                    break;
                case ErrorCode.Validation:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Validation Error",
                        Detail = error.Message ?? "Validation error",
                        Status = 422
                    };
                    break;
                case ErrorCode.UnprocessableEntity:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Unprocessable Entity",
                        Detail = error.Message ?? "Cannot process entity",
                        Status = 422
                    };
                    break;
                case ErrorCode.TooManyRequests:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Too Many Requests",
                        Detail = error.Message ?? "Too many requests",
                        Status = 429
                    };
                    break;
                case ErrorCode.ServiceUnavailable:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Service Unavailable",
                        Detail = error.Message ?? "Service unavailable",
                        Status = 503
                    };
                    break;
                case ErrorCode.InternalServerError:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = error.Message ?? "Internal server error",
                        Status = 500
                    };
                    break;
                default:
                    problemDetails = new ProblemDetails
                    {
                        Title = "Error",
                        Detail = error.Message ?? "Unknown error",
                        Status = 500
                    };
                    break;
            }

            return StatusCode(problemDetails.Status ?? 500, problemDetails);
        }
    }
}
