using MadiffTestAssignment.Exceptions;
using System.Net;
using System.Text.Json;

namespace MadiffTestAssignment.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
    private readonly IHostEnvironment _environment = environment;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.UnprocessableEntity, ex.Message);
        }
        catch (AppException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Katastrofalny błąd, szczegóły poniżej");
            var message = _environment.IsDevelopment() ? ex.Message : "Wewnętrzny błąd serwera";
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, message);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = message,
            status = (int)status,
            traceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
