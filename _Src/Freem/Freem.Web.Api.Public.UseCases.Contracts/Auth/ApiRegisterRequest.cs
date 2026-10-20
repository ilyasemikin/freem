using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Auth;

public sealed class ApiRegisterRequest : IRequest
{
    public RequestType Type => RequestTypes.Auth.Register;

    public RegisterRequest Body { get; }

    public ApiRegisterRequest(RegisterRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
