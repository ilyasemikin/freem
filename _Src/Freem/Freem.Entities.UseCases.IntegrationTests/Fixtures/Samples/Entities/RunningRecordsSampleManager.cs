using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
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

    public RunningRecord Start(UserIdentifier userId, ActivityIdentifier activityId, DateTimeOffset? startAt = null)
    {
        var context = new UseCaseExecutionContext(userId);
        
        var now = DateTime.UtcNow;
        startAt ??= now;
        var activities = new RelatedActivitiesCollection([activityId]);
        var request = new StartRunningRecordRequest(startAt.Value, activities);

        var response = _services.RequestExecutor.Execute<StartRunningRecordRequest, StartRunningRecordResponse>(context, request);
        if (!response.Success)
            throw new InvalidOperationException("Can't start running record");
        
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
    
    public bool TryGet(UserIdentifier userId, [NotNullWhen(true)] out RunningRecord? record)
    {
        var context = new UseCaseExecutionContext(userId);

        var request = new GetRunningRecordRequest();
        var response = _services.RequestExecutor.Execute<GetRunningRecordRequest, GetRunningRecordResponse>(context, request);

        if (!response.Success)
        {
            record = null;
            return false;
        }

        record = response.Record;
        return true;
    }
}