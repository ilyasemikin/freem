using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Auth;

public sealed class ApiRefreshRequest : IRequest
{
    public static readonly ApiRefreshRequest Instance = new();

    public RequestType Type => RequestTypes.Auth.Refresh;

    private ApiRefreshRequest()
    {
    }
}
