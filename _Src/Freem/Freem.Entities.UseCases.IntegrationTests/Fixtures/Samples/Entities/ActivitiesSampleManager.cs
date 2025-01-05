using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Archive.Models;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples.Entities;

public sealed class ActivitiesSampleManager
{
    private const string ActivityName = "activity";
    
    private readonly ServicesContext _services;

    public ActivitiesSampleManager(ServicesContext services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public Activity Create(UserIdentifier userId)
    {
        var request = new CreateActivityRequest(ActivityName);
        var context = new UseCaseExecutionContext(userId);
        
        var response = _services.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(context, request);

        return response.Activity;
    }

    public IEnumerable<Activity> CreateMany(UserIdentifier userId, int count)
    {
        var context = new UseCaseExecutionContext(userId);
        
        foreach (var index in Enumerable.Range(0, count))
        {
            var request = new CreateActivityRequest(ActivityName + index);
            
            var response = _services.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(context, request);
            yield return response.Activity;
        }
    }

    public void Archive(UserIdentifier userId, ActivityIdentifier activityId)
    {
        var context = new UseCaseExecutionContext(userId);

        var request = new ArchiveActivityRequest(activityId);
        _services.RequestExecutor.Execute(context, request);
    }
}