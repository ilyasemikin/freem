using Freem.Entities.Activities;
using Freem.Entities.Activities.Comparers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.Activities.List;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesListUseCase : UseCaseTestBase
{
    private const int ActivitiesCount = 10;

    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Activity> _activities;
    
    public ActivitiesListUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activities = services.Samples.Activities
            .CreateMany(userId, ActivitiesCount)
            .ToArray();

        _context = new UseCaseExecutionContext(userId);
        _activities = activities;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListActivityRequest();
        
        var response = await Services.RequestExecutor.ExecuteAsync<ListActivityRequest, ListActivityResponse>(_context, request);
        
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
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<ListActivityRequest, ListActivityResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}