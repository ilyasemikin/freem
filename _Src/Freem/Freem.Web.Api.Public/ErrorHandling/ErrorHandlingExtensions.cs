namespace Freem.Web.Api.Public.ErrorHandling;

internal static class ErrorHandlingExtensions
{
    public static IServiceCollection AddApiErrorHandling(this IServiceCollection services)
    {
        // UseExceptionHandler() requires AddProblemDetails() at startup even though
        // GlobalExceptionHandler always handles the exception itself.
        services.AddProblemDetails();
        return services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static IApplicationBuilder UseApiErrorHandling(this IApplicationBuilder application)
    {
        return application.UseExceptionHandler();
    }
}
