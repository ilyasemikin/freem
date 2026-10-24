using System.Net;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Users.Settings;
using Freem.Domain;
using Freem.Domain.Users.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Users;

// The EgoHub identity exists, but the Freem user has not been registered yet.
public sealed class UserNotRegisteredTests : TestBase
{
    public UserNotRegisteredTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Preparer.Users.AuthenticateAsync();
    }

    [Fact]
    public async Task Me_ShouldReturnUserNotRegisteredError()
    {
        var response = await Context.Client.Users.MeAsync();

        response.ShouldHaveClientError(HttpStatusCode.Unauthorized, DomainErrorCatalog.Types.UserNotRegistered);
    }

    [Fact]
    public async Task GetSettings_ShouldReturnUserNotRegisteredError()
    {
        var response = await Context.Client.Users.GetSettingsAsync();

        response.ShouldHaveClientError(HttpStatusCode.Unauthorized, DomainErrorCatalog.Types.UserNotRegistered);
    }

    [Fact]
    public async Task UpdateSettings_ShouldReturnUserNotRegisteredError()
    {
        var offset = TimeSpan.FromHours(1);
        var timeZoneOffset = new TimeZoneOffset(offset);
        var request = new UserSettingsUpdateRequest
        {
            TimeZoneOffset = new UpdateField<TimeZoneOffset>(timeZoneOffset)
        };

        var response = await Context.Client.Users.UpdateSettingsAsync(request);

        response.ShouldHaveClientError(HttpStatusCode.Unauthorized, DomainErrorCatalog.Types.UserNotRegistered);
    }
}
