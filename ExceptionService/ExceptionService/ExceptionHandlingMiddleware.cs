namespace ExceptionService;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            AppException appEx => (int)appEx.StatusCode,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        if (ex is AppException)
        {
            Log.Warning("App exception: {Message}", ex.Message);
        }
        else
        {
            Log.Error(ex, "❌ Unhandled exception");
        }
        
        var response = new ExceptionResponse
        {
            StatusCode = context.Response.StatusCode,
            Error = ex.GetType().Name,
            Message = ex.Message,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }


}