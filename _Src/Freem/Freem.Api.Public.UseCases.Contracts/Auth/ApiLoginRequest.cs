using Freem.Api.Public.Contracts.DTO.Auth;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Auth;

public sealed class ApiLoginRequest : IRequest
{
    public RequestType Type => RequestTypes.Auth.Login;

    public LoginRequest Body { get; }

    public ApiLoginRequest(LoginRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
