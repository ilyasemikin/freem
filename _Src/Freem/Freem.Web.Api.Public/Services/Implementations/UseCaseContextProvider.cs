using System.Security.Claims;
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
        
        var user = _accessor.HttpContext.User;
        var userId = user.FindFirstValue(JwtBearerAuthenticationClaimTypes.UserId);
        
        return userId is not null 
            ? new UseCaseExecutionContext(userId) 
            : UseCaseExecutionContext.Empty;
    }
}