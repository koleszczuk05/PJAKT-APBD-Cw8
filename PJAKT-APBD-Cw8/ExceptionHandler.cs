using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PJAKT_APBD_Cw8.Exceptions;

namespace WebApplication2;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            NotFoundException _ => StatusCodes.Status404NotFound,
            ConflictException _ => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Detail = statusCode == 500 ? "COs sie stalo" : exception.Message,
            Title = "Mamy problem"
        }, cancellationToken);

        return true;
    }
}