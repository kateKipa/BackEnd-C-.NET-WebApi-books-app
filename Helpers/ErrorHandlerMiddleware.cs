using BooksApiApp.Services.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace BooksApiApp.Helpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogDebug("Processing request in ErrorHandlerMiddleware.");
                await _next(context);
                
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = exception switch
                {

                    UserAlreadyExistsException or
                    PhonenumberAlreadyExistsException
                     => (int)HttpStatusCode.Conflict,

                    InvalidRegistrationException
                     => (int)HttpStatusCode.BadRequest,

                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    ForbiddenException => (int)HttpStatusCode.Forbidden,
                    UserNotFoundException or
                    TheListIsNullException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                //var result = JsonSerializer.Serialize(new { message = exception?.Message });
                var result = JsonSerializer.Serialize(new
                {
                    msg = "An error occurred",
                    errors = new { general = new[] { exception.Message } }
                });

                await response.WriteAsync(result);
            }
        }
    }
}

/* switch (error)
               {
                   case InvalidRegistrationException:
                       response.StatusCode = (int)HttpStatusCode.BadRequest;
                       break;
                   case KeyNotFoundException e:
                       // not found error
                       response.StatusCode = (int)HttpStatusCode.NotFound;
                       break;
                   default:
                       // unhandled error
                       response.StatusCode = (int)HttpStatusCode.InternalServerError;
                       break;
               }*/
