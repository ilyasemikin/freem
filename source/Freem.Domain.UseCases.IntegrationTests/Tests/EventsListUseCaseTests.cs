using Freem.Domain.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class EventsListUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    
    public EventsListUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();

        _context = new HandlerContext(userId);
    }

    [Fact]
    public async Task ShouldReturnUserRegisteredEvent_WhenUserIsRegistered()
    {
        var request = new ListEventRequest();

        var response = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        
        var @event = Assert.Single(response.Value.Events);
        Assert.Equal(EventTypes.Users.Registered, @event.Type);
        Assert.Equal(_context.UserId, @event.UserId);
        Assert.Equal(new EntityRef(EntityType.User, _context.UserId.ToString()), @event.EntityRef);
    }

    [Fact]
    public async Task ShouldReturnTagCreatedEvent_WhenFilteredByEntityType()
    {
        using var filler = Context.CreateExecutor();
        var tag = await filler.Tags.CreateAsync(_context);
        
        var request = new ListEventRequest
        {
            EntityType = EntityType.Tag
        };

        var response = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(_context, request);

        Assert.True(response.Success);
        
        var @event = Assert.Single(response.Value.Events);
        Assert.Equal(EventTypes.Tags.Created, @event.Type);
        Assert.Equal(new EntityRef(EntityType.Tag, tag.Id.ToString()), @event.EntityRef);
    }

    [Fact]
    public async Task ShouldReturnEmpty_WhenPeriodStartsInFuture()
    {
        var request = new ListEventRequest
        {
            From = DateTimeOffset.UtcNow.AddDays(1)
        };
        
        var response = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(_context, request);
        
        Assert.True(response.Success);
        Assert.Empty(response.Value.Events);
    }
}
