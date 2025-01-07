using Freem.Entities.Tags;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Tags.Create.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples.Entities;

public sealed class TagsSampleManager
{
    private readonly ServicesContext _services;

    public TagsSampleManager(ServicesContext services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public Tag Create(UserIdentifier userId)
    {
        var context = new UseCaseExecutionContext(userId);

        var name = Guid.NewGuid().ToString();
        var request = new CreateTagRequest(name);

        var response = _services.RequestExecutor.Execute<CreateTagRequest, CreateTagResponse>(context, request);
        return response.Tag;
    }

    public IEnumerable<Tag> CreateMany(UserIdentifier userId, int count)
    {
        var context = new UseCaseExecutionContext(userId);

        foreach (var _ in Enumerable.Range(0, count))
        {
            var name = Guid.NewGuid().ToString();
            var request = new CreateTagRequest(name);
            
            var response = _services.RequestExecutor.Execute<CreateTagRequest, CreateTagResponse>(context, request);
            yield return response.Tag;
        }
    }
}