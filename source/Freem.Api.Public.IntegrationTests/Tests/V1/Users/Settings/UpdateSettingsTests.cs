using System.Net;
using System.Net.Mime;
using System.Text;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Users.Settings;
using Freem.Domain;
using Freem.Domain.Users.Models;
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
        var value = new TimeZoneOffset(TimeSpan.FromHours(1));
        var request = new UserSettingsUpdateRequest
        {
            TimeZoneOffset = new UpdateField<TimeZoneOffset>(value)
        };

        var response = await Context.Client.Users.UpdateSettingsAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
    }

    [Fact]
    public async Task Request_ShouldKeepOffset_WhenUpdateWeekStart()
    {
        var offset = new TimeZoneOffset(TimeSpan.FromHours(2));
        var offsetRequest = new UserSettingsUpdateRequest
        {
            TimeZoneOffset = new UpdateField<TimeZoneOffset>(offset)
        };
        var weekStartRequest = new UserSettingsUpdateRequest
        {
            WeekStart = new UpdateField<WeekStart>(WeekStart.Sunday)
        };

        var offsetResponse = await Context.Client.Users.UpdateSettingsAsync(offsetRequest);
        offsetResponse.ShouldSuccess();
        var weekStartResponse = await Context.Client.Users.UpdateSettingsAsync(weekStartRequest);
        weekStartResponse.ShouldSuccess();

        var response = await Context.Client.Users.GetSettingsAsync();

        response.ShouldSuccess();
        Assert.Equal(offset, response.Value.TimeZoneOffset);
        Assert.Equal(WeekStart.Sunday, response.Value.WeekStart);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenHasNoChanges()
    {
        var request = new UserSettingsUpdateRequest();
        
        var response = await Context.Client.Users.UpdateSettingsAsync(request);

        Assert.NotNull(response);
        response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("14:30:00")]
    [InlineData("-13:00:00")]
    [InlineData("03:00:30")]
    public async Task Request_ShouldReturnInvalidRequestError_WhenOffsetIsInvalid(string offset)
    {
        using var client = Context.CreateHttpClient();
        var body = $$$"""{"timeZoneOffset":{"value":"{{{offset}}}"}}""";
        using var content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);

        using var response = await client.PutAsync("api/v1/user/settings", content);

        await response.ShouldHaveErrorAsync(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }

    [Theory]
    [InlineData("Tuesday")]
    [InlineData("1")]
    public async Task Request_ShouldReturnInvalidRequestError_WhenWeekStartIsInvalid(string weekStart)
    {
        using var client = Context.CreateHttpClient();
        var body = $$$"""{"weekStart":{"value":"{{{weekStart}}}"}}""";
        using var content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);

        using var response = await client.PutAsync("api/v1/user/settings", content);

        await response.ShouldHaveErrorAsync(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }
}
