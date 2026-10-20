using System.Net;
using Freem.Domain;
using Freem.Domain.Users.Models;
using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Web.Api.Public.Contracts.Models;
using Freem.Web.Api.Public.IntegrationTests.Context;
using Freem.Web.Api.Public.IntegrationTests.Extensions;
using Freem.Web.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Web.Api.Public.IntegrationTests.Tests.V1.Users;

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
        var dayUtcOffset = new DayUtcOffset(offset);
        var request = new UpdateUserSettingsRequest
        {
            DayUtcOffset = new UpdateField<DayUtcOffset>(dayUtcOffset)
        };

        var response = await Context.Client.Users.UpdateSettingsAsync(request);

        response.ShouldHaveClientError(HttpStatusCode.Unauthorized, DomainErrorCatalog.Types.UserNotRegistered);
    }
}
