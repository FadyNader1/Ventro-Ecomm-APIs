using Ecomm.core.Exceptions;
using Ecomm.Errors;
using System.Text.Json;

namespace Ecomm.Middleware
{
    public class MiddlewareHandleError
    {
        private readonly RequestDelegate next;
        private readonly ILogger<MiddlewareHandleError> logger;
        private readonly IWebHostEnvironment env;

        public MiddlewareHandleError(RequestDelegate next, ILogger<MiddlewareHandleError> logger, IWebHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);

            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            object response;
            switch (exception)
            {
                case BadRequestException badRequestException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = env.IsDevelopment() ?
                        new ApiServerError(400, badRequestException.Message.ToString(), exception.StackTrace)
                        :
                        new ApiServerError(400, badRequestException.Message.ToString());
                    break;
                case NotFoundException notFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = env.IsDevelopment() ?
                        new ApiServerError(404, notFoundException.Message.ToString(), exception.StackTrace)
                        :
                        new ApiServerError(404, notFoundException.Message.ToString());
                    break;
                case UnAuthorizeException unAuthorizeException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response = env.IsDevelopment() ?
                        new ApiServerError(401, unAuthorizeException.Message.ToString(), exception.StackTrace)
                        :
                        new ApiServerError(401, unAuthorizeException.Message.ToString());
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = env.IsDevelopment() ?
                        new ApiServerError(500, exception.Message.ToString(), exception.StackTrace)
                        :
                        new ApiServerError(500, "Internal Server Error");
                    break;
            }
            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}