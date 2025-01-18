using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.Users.Identifiers;
using Freem.Time.Models;
using Record = Freem.Entities.Records.Record;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples.Entities;

public sealed class RecordsSampleManager
{
    private readonly ServicesContext _services;

    public RecordsSampleManager(ServicesContext services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public Record Create(UserIdentifier userId, ActivityIdentifier activityId)
    {
        var context = new UseCaseExecutionContext(userId);

        var now = DateTimeOffset.UtcNow;
        var period = new DateTimePeriod(now.AddHours(-1), now);
        var activities = new RelatedActivitiesCollection([activityId]);
        var request = new CreateRecordRequest(period, activities);

        var response = _services.RequestExecutor.Execute<CreateRecordRequest, CreateRecordResponse>(context, request);
        return response.Record;
    }

    public IEnumerable<Record> CreateMany(UserIdentifier userId, ActivityIdentifier activityId, int count)
    {
        var context = new UseCaseExecutionContext(userId);

        foreach (var _ in Enumerable.Range(0, count))
        {
            var now = DateTimeOffset.UtcNow;
            
            var period = new DateTimePeriod(now.AddHours(-1), now);
            var activities = new RelatedActivitiesCollection([activityId]);
            
            var request = new CreateRecordRequest(period, activities);
            var response = _services.RequestExecutor.Execute<CreateRecordRequest, CreateRecordResponse>(context, request);
            
            yield return response.Record;
        }
    }

    public Record Get(UserIdentifier userId, RecordIdentifier recordId)
    {
        var context = new UseCaseExecutionContext(userId);
        var request = new GetRecordRequest(recordId);
        
        var response = _services.RequestExecutor.Execute<GetRecordRequest, GetRecordResponse>(context, request);
        if (!response.Success)
            throw new InvalidOperationException("Record not found");
        
        return response.Record;
    }
}