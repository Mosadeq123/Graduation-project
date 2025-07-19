using Microsoft.AspNetCore.Http;
using Store.G04.APIs.Errors;
using System.Text.Json;

namespace Store.G04.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context) // تم تصحيح النوع إلى HttpContext
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = _env.IsDevelopment() ?
                    new ApiEceptionResponse(StatusCodes.Status500InternalServerError,ex.Message, ex?.StackTrace?.ToString())
                    : new ApiEceptionResponse(StatusCodes.Status500InternalServerError);
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var Json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(Json);
                
            }
        }
    }
}

