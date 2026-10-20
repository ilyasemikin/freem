using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Auth;

public sealed class ApiLogoutRequest : IRequest
{
    public static readonly ApiLogoutRequest Instance = new();

    public RequestType Type => RequestTypes.Auth.Logout;

    private ApiLogoutRequest()
    {
    }
}
