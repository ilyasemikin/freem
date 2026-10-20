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
    public async Task ShouldSuccess_WhenAfterNull()
    {
        var request = new ListEventRequest();

        var response = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Events);
        Assert.Null(response.Error);
        
        Assert.NotEmpty(response.Value.Events);
    }

    [Fact]
    public async Task ShouldSuccess_WhenAfterNotNull()
    {
        var now = DateTime.UtcNow;
        var request = new ListEventRequest()
        {
            After = now
        };
        
        var response = await Context.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Events);
        Assert.Null(response.Error);
        
        Assert.Empty(response.Value.Events);
    }
    
}