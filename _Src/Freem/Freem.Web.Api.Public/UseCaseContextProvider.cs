using System.Security.Claims;
using Freem.Entities.UseCases;

namespace Freem.Web.Api.Public;

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
        var userId = user.FindFirstValue("UserId");
        
        return userId is not null 
            ? new UseCaseExecutionContext(userId) 
            : UseCaseExecutionContext.Empty;
    }
}