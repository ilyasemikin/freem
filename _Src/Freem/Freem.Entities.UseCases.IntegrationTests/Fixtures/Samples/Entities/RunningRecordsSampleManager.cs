using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.RunningRecords.Get.Models;
using Freem.Entities.UseCases.RunningRecords.Remove.Models;
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

    public void Remove(UserIdentifier userId)
    {
        var context = new UseCaseExecutionContext(userId);

        var request = new RemoveRunningRecordRequest();
        
        _services.RequestExecutor.Execute<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(context, request);
    }

    public RunningRecord Get(UserIdentifier userId)
    {
        var context = new UseCaseExecutionContext(userId);

        var request = new GetRunningRecordRequest();
        var response = _services.RequestExecutor.Execute<GetRunningRecordRequest, GetRunningRecordResponse>(context, request);

        if (!response.Success)
            throw new InvalidOperationException("Can't get running record");
        
        return response.Record;
    }
}