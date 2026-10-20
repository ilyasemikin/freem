using Freem.Domain.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Events.Get;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class EventsGetUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private HandlerContext _otherContext = null!;

    public EventsGetUseCaseTests(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        var otherUserId = await filler.Users.RegisterAsync();

        _context = new HandlerContext(userId);
        _otherContext = new HandlerContext(otherUserId);
    }

    [Fact]
    public async Task ShouldSuccess_WhenEventBelongsToUser()
    {
        var id = await GetFirstEventIdAsync(_context);
        var request = new GetEventRequest(id);

        var response = await Context.ExecuteAsync<GetEventRequest, Result<GetEventResponse, Error>>(_context, request);

        Assert.True(response.Success);
        Assert.Equal(id, response.Value.Event.Id);
        Assert.Equal(_context.UserId, response.Value.Event.UserId);
    }

    [Fact]
    public async Task ShouldReturnEntityNotFound_WhenEventNotExists()
    {
        var request = new GetEventRequest(EventId.Generate());

        var response = await Context.ExecuteAsync<GetEventRequest, Result<GetEventResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldReturnEntityNotFound_WhenEventBelongsToOtherUser()
    {
        var id = await GetFirstEventIdAsync(_otherContext);
        var request = new GetEventRequest(id);

        var response = await Context.ExecuteAsync<GetEventRequest, Result<GetEventResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, response.Error.Type);
    }

    private async Task<EventId> GetFirstEventIdAsync(HandlerContext context)
    {
        var request = new ListEventRequest();
        var response = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(context, request);

        Assert.True(response.Success);
        return response.Value.Events[0].Id;
    }
}
