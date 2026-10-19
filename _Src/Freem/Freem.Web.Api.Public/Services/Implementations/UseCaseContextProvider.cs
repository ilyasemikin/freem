using Freem.Domain.Users;
using Freem.Entities.UseCases;
using Freem.Web.Api.Public.Authentication;

namespace Freem.Web.Api.Public.Services.Implementations;

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

        var value = _accessor.HttpContext.User.FindFirst(AuthenticationClaimTypes.UserId)?.Value;

        return UserId.TryParse(value, null, out var userId)
            ? new UseCaseExecutionContext(userId)
            : UseCaseExecutionContext.Empty;
    }
}
