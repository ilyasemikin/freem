using Freem.Api.Public.Contracts.V1.Users.Settings;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Users.Settings;

public sealed class ApiUpdateUserSettingsRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Settings.Update;

    public UserSettingsUpdateRequest Body { get; }

    public ApiUpdateUserSettingsRequest(UserSettingsUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
