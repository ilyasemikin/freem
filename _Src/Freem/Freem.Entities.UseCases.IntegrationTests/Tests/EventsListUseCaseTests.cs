using Freem.Entities.UseCases.Contracts.Events.List;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class EventsListUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    
    public EventsListUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();

        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess_WhenAfterNull()
    {
        var request = new ListEventRequest();

        var response = await Context.ExecuteAsync<ListEventRequest, ListEventResponse>(_context, request);

        Assert.NotNull(response);
        Assert.NotNull(response.Events);
        Assert.Null(response.Error);
        
        Assert.NotEmpty(response.Events);
    }

    [Fact]
    public async Task ShouldSuccess_WhenAfterNotNull()
    {
        var now = DateTime.UtcNow;
        var request = new ListEventRequest()
        {
            After = now
        };
        
        var response = await Context.ExecuteAsync<ListEventRequest, ListEventResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Events);
        Assert.Null(response.Error);
        
        Assert.Empty(response.Events);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new ListEventRequest();
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<ListEventRequest, ListEventResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}