using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Users;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Authentication;

namespace Freem.Web.Api.Public.Services.Implementations;

public sealed class UseCaseContextProvider
{
    private readonly IHttpContextAccessor _accessor;
    
    public UseCaseContextProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public bool TryGet([NotNullWhen(true)] out HandlerContext? context)
    {
        context = null;
        if (_accessor.HttpContext is null)
            return false;

        var value = _accessor.HttpContext.User.FindFirst(AuthenticationClaimTypes.UserId)?.Value;
        if (!UserId.TryParse(value, null, out var userId))
            return false;

        context = new HandlerContext(userId);
        return true;
    }
}
