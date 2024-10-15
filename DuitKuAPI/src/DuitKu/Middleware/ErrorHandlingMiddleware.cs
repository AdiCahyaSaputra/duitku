using System.Net;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using NuGet.Protocol;

namespace DuitKu.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IConfiguration config)
        {
            _next = next;
            _logger = logger;
            _configuration = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                _logger.LogInformation($"API Hit status code {context.Response.StatusCode} Method {context.Request.Method} Url {context.Request.Path}");

                var statusCode = context.Response.StatusCode;

                if (statusCode == StatusCodes.Status404NotFound)
                {
                    context.Response.ContentType = "application/json";
                    var response = new ProblemDetails
                    {
                        Title = "Waduh gak nemu ni bre",
                        Status = StatusCodes.Status404NotFound,
                        Instance = context.Request.Path,
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        title = response.Title,
                        status = response.Status,
                        instance = response.Instance,
                    }));
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
                _logger.LogError(ex.ToJson());

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}