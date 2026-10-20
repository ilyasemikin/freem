using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Users.Get;

public sealed class GetUserRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Get;

    public static GetUserRequest Instance { get; } = new();
}