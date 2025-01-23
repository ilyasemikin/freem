using Freem.Entities.Activities;
using Freem.Entities.Activities.Comparers;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesListUseCase : UseCaseTestBase
{
    private const int ActivitiesCount = 10;

    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Activity> _activities;
    
    public ActivitiesListUseCase(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);

        var activities = filler.Activities.CreateMany(_context, ActivitiesCount);
        _activities = activities
            .OrderBy(e => (string)e.Name)
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListActivityRequest();
        
        var response = await Context.ExecuteAsync<ListActivityRequest, ListActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Activities);
        Assert.NotNull(response.TotalCount);
        Assert.Null(response.Error);
        
        var orderedActivities = response.Activities
            .OrderBy(activity => (string)activity.Name)
            .ToArray();
        
        Assert.Equal(ActivitiesCount, (int)response.TotalCount);
        Assert.Equal(_activities, orderedActivities, new ActivityEqualityComparer());
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new ListActivityRequest();
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<ListActivityRequest, ListActivityResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}