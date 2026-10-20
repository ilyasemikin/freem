using Freem.Web.Api.Public.Contracts.DTO.Users;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Users;

public sealed class ApiRegisterUserRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Register;

    public RegisterUserRequest Body { get; }

    public ApiRegisterUserRequest(RegisterUserRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
