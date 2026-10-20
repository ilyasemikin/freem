using System.Net;
using Freem.Domain.Users.Models;
using Freem.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Api.Public.Contracts.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Users.Settings.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Users.Settings;

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
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldFail_WhenHasNoChanges()
    {
        var request = new UpdateUserSettingsRequest();
        
        var response = await Context.Client.Users.UpdateSettingsAsync(request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }
}