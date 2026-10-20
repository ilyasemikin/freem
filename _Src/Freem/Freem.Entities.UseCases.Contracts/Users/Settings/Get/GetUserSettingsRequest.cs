using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Users.Settings.Get;

public sealed class GetUserSettingsRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Settings.Get;

    public static GetUserSettingsRequest Instance { get; } = new();
}