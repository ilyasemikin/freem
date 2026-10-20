using Freem.Domain.Events;
using Freem.Api.Public.Contracts.DTO.Events;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Events.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Events;

public sealed class ListEventTests : EventTestBase
{
    public ListEventTests(TestContext context, ITestOutputHelper? output = null) : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new ListEventRequest(ListEventRequest.DefaultOffset, ListEventRequest.DefaultLimit);

        var response = await Context.Client.Events.ListAsync(request);

        Assert.NotNull(response);
        response.ShouldSuccess();
        Assert.NotEmpty(response.Value);
    }

    [Fact]
    public async Task Request_ShouldReturnEmpty_WhenFilteredByEntityTypeWithoutEvents()
    {
        var request = new ListEventRequest(
            ListEventRequest.DefaultOffset,
            ListEventRequest.DefaultLimit,
            EntityType.Tag);

        var response = await Context.Client.Events.ListAsync(request);

        response.ShouldSuccess();
        Assert.Empty(response.Value);
    }

    [Fact]
    public async Task Request_ShouldReturnEmpty_WhenPeriodStartsInFuture()
    {
        var from = DateTimeOffset.UtcNow.AddDays(1);
        var request = new ListEventRequest(
            ListEventRequest.DefaultOffset,
            ListEventRequest.DefaultLimit,
            from: from);

        var response = await Context.Client.Events.ListAsync(request);

        response.ShouldSuccess();
        Assert.Empty(response.Value);
    }
}
