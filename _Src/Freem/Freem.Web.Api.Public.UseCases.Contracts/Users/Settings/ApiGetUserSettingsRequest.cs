using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Users.Settings;

public sealed class ApiGetUserSettingsRequest : IRequest
{
    public static readonly ApiGetUserSettingsRequest Instance = new();

    public RequestType Type => RequestTypes.Users.Settings.Get;

    private ApiGetUserSettingsRequest()
    {
    }
}
