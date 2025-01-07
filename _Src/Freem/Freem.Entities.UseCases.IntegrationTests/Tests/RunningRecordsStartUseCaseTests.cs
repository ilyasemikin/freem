using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.RunningRecords.Start.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStartUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _activityId;
    
    public RunningRecordsStartUseCaseTests(ServicesContext services) 
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
        var activities = new RelatedActivitiesCollection([_activityId]);
        
        var request = new StartRunningRecordRequest(now, activities);
        await Services.RequestExecutor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(_context, request);
    }
}