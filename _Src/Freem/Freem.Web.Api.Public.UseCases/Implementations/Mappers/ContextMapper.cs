using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Context;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers;

internal static class ContextMapper
{
    public static HandlerContext Map(this ApiAuthorizedHandlerContext context)
    {
        return new HandlerContext(context.UserId, context.RequestId);
    }
}
