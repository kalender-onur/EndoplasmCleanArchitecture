namespace EndoplasmCleanArchitecture.Api.Middleware
{
    using Microsoft.AspNetCore.Http;
    using EndoplasmCleanArchitecture.Api.DTOs;
    using EndoplasmCleanArchitecture.Domain.Entities;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = Response<string>.ErrorResponse(ex.Message, response.StatusCode);
                _logger.LogError($"Stack Trace: {ex.StackTrace} Inner Exception: {ex.InnerException} Message: {ex.Message}");
                await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }

    }


}
