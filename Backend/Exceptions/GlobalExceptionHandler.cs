using Microsoft.AspNetCore.Diagnostics;

namespace Backend.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is NotFoundException)
        {
            httpContext.Response.StatusCode = 404;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                erro = exception.Message
            }, cancellationToken);

            return true;
        }
        if (exception is BadRequestException)
        {
            httpContext.Response.StatusCode = 400;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                erro = exception.Message
            },cancellationToken);

            return true;
        }
        if (exception is ConflictException)
        {
            httpContext.Response.StatusCode = 409;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                erro = exception.Message
            }, cancellationToken);

            return true;
        }
        if (exception is UnauthorizedException)
        {
            httpContext.Response.StatusCode = 401;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                erro = exception.Message
            }, cancellationToken);

            return true;
        }
        if (exception is ForbiddenException)
        {
            httpContext.Response.StatusCode = 403;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                erro = exception.Message
            }, cancellationToken);

            return true;
        }
        return false;
    }
    
    
}