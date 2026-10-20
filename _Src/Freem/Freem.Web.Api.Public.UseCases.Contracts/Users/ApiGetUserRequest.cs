using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Users;

public sealed class ApiGetUserRequest : IRequest
{
    public static readonly ApiGetUserRequest Instance = new();

    public RequestType Type => RequestTypes.Users.Get;

    private ApiGetUserRequest()
    {
    }
}
