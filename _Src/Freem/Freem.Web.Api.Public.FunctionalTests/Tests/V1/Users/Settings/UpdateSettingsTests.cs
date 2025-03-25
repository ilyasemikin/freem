using System.Net;
using Freem.Entities.Users.Models;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Users.Settings;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.Settings.Base;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.Settings;

public sealed class UpdateSettingsTests : SettingsTestBase
{
    public UpdateSettingsTests(TestContext context) 
        : base(context)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var value = new DayUtcOffset(TimeSpan.FromHours(1));
        var request = new UpdateUserSettingsRequest
        {
            DayUtcOffset = new UpdateField<DayUtcOffset>(value)
        };

        var response = await Context.Client.Users.UpdateSettingsAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenHasNoChanges()
    {
        var request = new UpdateUserSettingsRequest();
        
        var response = await Context.Client.Users.UpdateSettingsAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}