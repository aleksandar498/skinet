using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class EceptionMiddleware
    {
        private readonly ILogger<EceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly RequestDelegate _next;
        public EceptionMiddleware(RequestDelegate next, ILogger<EceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                ? new ApiExceptions((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace.ToString()) :
                new ApiExceptions((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}