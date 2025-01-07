using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Events.List.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class EventsListUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    
    public EventsListUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();

        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess_WhenAfterNull()
    {
        var request = new ListEventRequest();

        var response = await Services.RequestExecutor.ExecuteAsync<ListEventRequest, ListEventResponse>(_context, request);

        Assert.NotNull(response);
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
        
        var response = await Services.RequestExecutor.ExecuteAsync<ListEventRequest, ListEventResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.Empty(response.Events);
    }
}