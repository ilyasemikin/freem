using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Fixtures.Executors;

internal static class ResultExtensions
{
    public static void EnsureSuccess(this Result<Error> result)
    {
        if (!result.Success)
            throw new InvalidOperationException(result.Error.Message);
    }

    public static T EnsureSuccess<T>(this Result<T, Error> result)
    {
        if (!result.Success)
            throw new InvalidOperationException(result.Error.Message);

        return result.Value;
    }
}