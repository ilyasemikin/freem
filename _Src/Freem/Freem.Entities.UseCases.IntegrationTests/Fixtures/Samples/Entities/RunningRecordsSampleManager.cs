using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.RunningRecords.Start.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples.Entities;

public sealed class RunningRecordsSampleManager
{
    private readonly ServicesContext _services;

    public RunningRecordsSampleManager(ServicesContext services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public RunningRecord Start(UserIdentifier userId, ActivityIdentifier activityId)
    {
        var context = new UseCaseExecutionContext(userId);
        
        var now = DateTime.UtcNow;
        var activities = new RelatedActivitiesCollection([activityId]);
        var request = new StartRunningRecordRequest(now, activities);

        var response = _services.RequestExecutor.Execute<StartRunningRecordRequest, StartRunningRecordResponse>(context, request);
        return response.RunningRecord;
    }
}