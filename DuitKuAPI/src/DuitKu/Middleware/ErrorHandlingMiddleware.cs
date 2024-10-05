using System.Net;

using Newtonsoft.Json;

namespace DuitKu.Middleware
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

                _logger.LogInformation($"API Hit status code {context.Response.StatusCode} Method {context.Request.Method} Url {context.Request.Path}");

                var statusCode = context.Response.StatusCode;

                if (statusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        message = "Login dulu ya bre"
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }

                if(statusCode == StatusCodes.Status404NotFound) {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        message = "Wah gak nemu data nya nih"
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    message = "Waduh ada error dari server ni bre",
                };

                _logger.LogError(ex.Message);

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}