using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Users.Settings;

public sealed class ApiUpdateUserSettingsRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Settings.Update;

    public UpdateUserSettingsRequest Body { get; }

    public ApiUpdateUserSettingsRequest(UpdateUserSettingsRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
