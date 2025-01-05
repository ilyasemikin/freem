using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsCreateUseCase : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";
    
    private const string ActivityName = "activity";

    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _activityId;
    
    public RecordsCreateUseCase(ServicesContext context) 
        : base(context)
    {
        var registerRequest = new RegisterUserPasswordRequest(Nickname, Login, Password);
        var registerResponse = Context.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, registerRequest);
        
        _context = new UseCaseExecutionContext(registerResponse.UserId);

        var activityRequest = new CreateActivityRequest(ActivityName);
        var activityResponse = Context.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(_context, activityRequest);

        _activityId = activityResponse.Activity.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var now = DateTime.UtcNow;
        var period = new DateTimePeriod(now.AddHours(-4), now);
        var activities = new RelatedActivitiesCollection([_activityId]);
        
        var request = new CreateRecordRequest(period, activities);
        
        var response = await Context.RequestExecutor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Record.Id);
        Assert.Equal(_context.UserId, response.Record.UserId);
        Assert.Null(response.Record.Name);
        Assert.Null(response.Record.Description);
        Assert.Equal(activities, response.Record.Activities, IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals);
    }
}