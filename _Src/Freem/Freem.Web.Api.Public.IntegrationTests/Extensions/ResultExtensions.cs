using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Web.Api.Public.IntegrationTests.Extensions;

public static class ResultExtensions
{
    public static void EnsureSuccess(this IResult<IClientError> result)
    {
        if (!result.Success)
            throw new InvalidOperationException(result.Error.Message);
    }

    public static T Unwrap<T>(this Result<T, IClientError> result)
    {
        return result.Success
            ? result.Value
            : throw new InvalidOperationException(result.Error.Message);
    }
}
