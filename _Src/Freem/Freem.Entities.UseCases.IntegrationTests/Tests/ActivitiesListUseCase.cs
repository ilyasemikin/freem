using Freem.Domain.Entities.Activities;
using Freem.Domain.Entities.Activities.Comparers;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesListUseCase : UseCaseTestBase
{
    private const int ActivitiesCount = 10;

    private HandlerContext _context = null!;
    private IReadOnlyList<Activity> _activities = null!;
    
    public ActivitiesListUseCase(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);

        var activities = await filler.Activities.CreateManyAsync(_context, ActivitiesCount);
        _activities = activities
            .OrderBy(e => e.Name.ToString())
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListActivityRequest();
        
        var response = await Context.ExecuteAsync<ListActivityRequest, Result<ListActivityResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Activities);
        Assert.NotNull(response.Value.TotalCount);
        Assert.Null(response.Error);
        
        var orderedActivities = response.Value.Activities
            .OrderBy(activity => activity.Name.ToString())
            .ToArray();
        
        Assert.Equal(ActivitiesCount, (int)response.Value.TotalCount);
        Assert.Equal(_activities, orderedActivities, new ActivityEqualityComparer());
    }
    
}