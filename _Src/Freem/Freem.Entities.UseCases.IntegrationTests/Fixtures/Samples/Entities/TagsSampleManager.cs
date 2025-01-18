using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.Get;
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

    public Tag Create(UserIdentifier userId, string? name = null)
    {
        var context = new UseCaseExecutionContext(userId);

        name ??= Guid.NewGuid().ToString();
        var request = new CreateTagRequest(name);

        var response = _services.RequestExecutor.Execute<CreateTagRequest, CreateTagResponse>(context, request);
        if (!response.Success)
            throw new InvalidOperationException("Can't create tag");
        
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

    public Tag Get(UserIdentifier userId, TagIdentifier tagId)
    {
        var context = new UseCaseExecutionContext(userId);

        var request = new GetTagRequest(tagId);
        
        var response = _services.RequestExecutor.Execute<GetTagRequest, GetTagResponse>(context, request);
        if (!response.Success)
            throw new InvalidOperationException("Can't get tag");
        
        return response.Tag;
    }
}