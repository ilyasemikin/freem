using System.Net;
using Freem.Domain;
using EventId = Freem.Domain.Events.EventId;
using Freem.Api.Public.Contracts.DTO.Events;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.V1.Events.Base;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Events;

public sealed class GetEventTests : EventTestBase
{
    public GetEventTests(TestContext context, ITestOutputHelper? output = null) : base(context, output)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var query = new ListEventRequest(ListEventRequest.DefaultOffset, ListEventRequest.DefaultLimit);
        var events = await Context.Client.Events.ListAsync(query);
        var id = events.Unwrap()[0].Id;

        var response = await Context.Client.Events.GetAsync(id);

        response.ShouldSuccess();
        Assert.Equal(id, response.Value.Id);
    }

    [Fact]
    public async Task Request_ShouldReturnEntityNotFoundError_WhenEventNotExists()
    {
        var id = EventId.Generate();

        var response = await Context.Client.Events.GetAsync(id);

        response.ShouldHaveClientError(HttpStatusCode.NotFound, DomainErrorCatalog.Types.EntityNotFound);
    }
}
