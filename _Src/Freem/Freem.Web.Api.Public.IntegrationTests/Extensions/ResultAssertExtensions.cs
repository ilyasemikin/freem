using System.Net;
using Freem.Web.Api.Public.Client.Errors;
using Utilities.Errors;
using Utilities.Http.Errors;
using Utilities.Http.Errors.Abstractions;
using Utilities.Primitives;

namespace Freem.Web.Api.Public.IntegrationTests.Extensions;

public static class ResultAssertExtensions
{
    public static void ShouldSuccess(this IResult<IClientError> result)
    {
        Assert.True(result.Success, result.Error?.Message);
    }

    public static void ShouldHaveStatusCode(this IResult<IClientError> result, HttpStatusCode statusCode)
    {
        Assert.False(result.Success);

        var actual = result.Error switch
        {
            ClientError error => error.Response.StatusCode,
            HttpClientError error => error.Response.StatusCode,
            _ => throw new InvalidOperationException(result.Error.Message)
        };

        Assert.Equal(statusCode, actual);
    }

    public static void ShouldHaveClientError(this IResult<IClientError> result, HttpStatusCode statusCode, ErrorType type)
    {
        Assert.False(result.Success);

        var error = Assert.IsType<ClientError>(result.Error);
        Assert.Equal(statusCode, error.Response.StatusCode);
        Assert.Equal(type, error.Error.Type);
    }
}
