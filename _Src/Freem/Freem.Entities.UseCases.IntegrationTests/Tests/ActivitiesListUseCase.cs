using Freem.Entities.Activities;
using Freem.Entities.Activities.Comparers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.Activities.List.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesListUseCase : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";

    private const string ActivityNamePrefix = "activity";
    private const int ActivitiesCount = 10;

    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Activity> _activities;
    
    public ActivitiesListUseCase(ServicesContext context) 
        : base(context)
    {
        var registerRequest = new RegisterUserPasswordRequest(Nickname, Login, Password);
        var registerResponse = Context.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, registerRequest);

        _context = new UseCaseExecutionContext(registerResponse.UserId);
        
        var activities = new List<Activity>();
        foreach (var index in Enumerable.Range(0, ActivitiesCount))
        {
            var name = ActivityNamePrefix + index;
            var activityRequest = new CreateActivityRequest(name);
            var activityResponse = Context.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(_context, activityRequest);

            activities.Add(activityResponse.Activity);
        }
        
        _activities = activities;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListActivityRequest();
        
        var response = await Context.RequestExecutor.ExecuteAsync<ListActivityRequest, ListActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        
        var orderedActivities = response.Activities
            .OrderBy(activity => (string)activity.Name)
            .ToArray();
        
        Assert.Equal(ActivitiesCount, (int)response.TotalCount);
        Assert.Equal(_activities, orderedActivities, new ActivityEqualityComparer());
    }
}