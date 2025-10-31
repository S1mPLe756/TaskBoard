using Microsoft.AspNetCore.Builder;

namespace ExceptionService;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseExceptionService(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();

}