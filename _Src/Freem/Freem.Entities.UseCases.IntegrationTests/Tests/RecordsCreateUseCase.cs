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
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _activityId;
    
    public RecordsCreateUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _activityId = activity.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var now = DateTime.UtcNow;
        var period = new DateTimePeriod(now.AddHours(-4), now);
        var activities = new RelatedActivitiesCollection([_activityId]);
        
        var request = new CreateRecordRequest(period, activities);
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Record.Id);
        Assert.Equal(_context.UserId, response.Record.UserId);
        Assert.Null(response.Record.Name);
        Assert.Null(response.Record.Description);
        Assert.Equal(activities, response.Record.Activities, IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals);
    }
}