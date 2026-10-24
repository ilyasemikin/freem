using Freem.Domain.UseCases.Context;
using Freem.Api.Public.UseCases.Context;

namespace Freem.Api.Public.UseCases.Implementations.Mappers;

internal static class ContextMapper
{
    public static HandlerContext Map(this ApiAuthorizedHandlerContext context)
    {
        return new HandlerContext(context.UserId, context.RequestId);
    }
}
