using System.Net;
using System.Text.Json;

namespace HabitLoggerMvc.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var isHtmlRequest = context.Request.Headers.Accept.ToString().Contains("text/html");

        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An internal server error occurred.";

        if (exception is KeyNotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            message = exception.Message;
        }

        if (isHtmlRequest)
        {
            context.Response.Redirect($"/Error?message={WebUtility.UrlEncode(message)}");
            return Task.CompletedTask;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}
