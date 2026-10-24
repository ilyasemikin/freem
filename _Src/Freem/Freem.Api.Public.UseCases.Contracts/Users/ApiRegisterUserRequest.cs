using Freem.Api.Public.Contracts.V1.Users;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Users;

public sealed class ApiRegisterUserRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Register;

    public UserRegisterRequest Body { get; }

    public ApiRegisterUserRequest(UserRegisterRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
