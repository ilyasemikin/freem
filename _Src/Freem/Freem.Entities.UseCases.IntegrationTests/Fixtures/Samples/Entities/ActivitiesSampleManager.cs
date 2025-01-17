using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.DTO.Activities.Archive;
using Freem.Entities.UseCases.DTO.Activities.Create;
using Freem.Entities.UseCases.DTO.Activities.Get;
using Freem.Entities.UseCases.DTO.Activities.Unarchive;
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
        var context = new UseCaseExecutionContext(userId);
        var request = new CreateActivityRequest(ActivityName);
        
        var response = _services.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(context, request);
        if (!response.Success)
            throw new InvalidOperationException("Can't create activity");
        
        return response.Activity;
    }

    public IEnumerable<Activity> CreateMany(UserIdentifier userId, int count)
    {
        var context = new UseCaseExecutionContext(userId);
        
        foreach (var index in Enumerable.Range(0, count))
        {
            var request = new CreateActivityRequest(ActivityName + index);
            
            var response = _services.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(context, request);
            if (!response.Success)
                throw new InvalidOperationException("Can't create activity");
            
            yield return response.Activity;
        }
    }

    public void Archive(UserIdentifier userId, ActivityIdentifier activityId)
    {
        var context = new UseCaseExecutionContext(userId);
        var request = new ArchiveActivityRequest(activityId);
        
        _services.RequestExecutor.Execute<ArchiveActivityRequest, ArchiveActivityResponse>(context, request);
    }

    public void Unarchive(UserIdentifier userId, ActivityIdentifier activityId)
    {
        var context = new UseCaseExecutionContext(userId);
        var request = new UnarchiveActivityRequest(activityId);
        
        _services.RequestExecutor.Execute<UnarchiveActivityRequest, UnarchiveActivityResponse>(context, request);
    }

    public Activity Get(UserIdentifier userId, ActivityIdentifier activityId)
    {
        var context = new UseCaseExecutionContext(userId);
        var request = new GetActivityRequest(activityId);
        
        var response = _services.RequestExecutor.Execute<GetActivityRequest, GetActivityResponse>(context, request);
        if (!response.Success)
            throw new InvalidOperationException($"Can't get activity {activityId}");
        
        return response.Activity;
    }
}