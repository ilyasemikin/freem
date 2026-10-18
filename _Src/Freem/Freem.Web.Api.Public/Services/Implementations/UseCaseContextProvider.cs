using Freem.Entities.UseCases;
using Freem.Web.Api.Public.Contracts;

namespace Freem.Web.Api.Public.Services.Implementations;

// Temporary scheme until EgoHub integration: the user id is taken from the query string.
public sealed class UseCaseContextProvider
{
    private readonly IHttpContextAccessor _accessor;
    
    public UseCaseContextProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public UseCaseExecutionContext Get()
    {
        if (_accessor.HttpContext is null)
            return UseCaseExecutionContext.Empty;

        var userId = _accessor.HttpContext.Request.Query[QueryParameterNames.UserId].ToString();

        return !string.IsNullOrWhiteSpace(userId)
            ? new UseCaseExecutionContext(userId)
            : UseCaseExecutionContext.Empty;
    }
}
