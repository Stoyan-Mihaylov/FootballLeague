using FootballLeague.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FootballLeague.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            object errorResponse;

            switch (exception)
            {
                case NotFoundException nf:
                    statusCode = StatusCodes.Status404NotFound;
                    errorResponse = new
                    {
                        title = "Resource Not Found",
                        status = statusCode,
                        detail = nf.Message
                    };
                    break;

                case BadRequestException bre:
                    statusCode = StatusCodes.Status400BadRequest;
                    errorResponse = new
                    {
                        title = "Bad Request",
                        status = statusCode,
                        detail = bre.Message
                    };
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;

                    _logger.LogError(exception, "Unhandled exception occurred.");

                    errorResponse = new
                    {
                        title = "Internal Server Error",
                        status = statusCode,
                        detail = "An unexpected error occurred."
                    };
                    break;
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, _jsonOptions));
        }
    }
}
