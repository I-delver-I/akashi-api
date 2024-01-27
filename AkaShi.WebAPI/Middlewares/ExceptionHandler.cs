using System.Net;

namespace AkaShi.WebAPI.Middlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        
        switch (exception)
        {
            case KeyNotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case ArgumentNullException: case ArgumentException argumentException:
                code = HttpStatusCode.BadRequest;
                break;
        }

        context.Response.ContentType = "application/text";
        context.Response.StatusCode = (int)code;
        
        if (result == string.Empty)
        {
            result = exception.Message;
        }

        return context.Response.WriteAsync(result);
    }
}