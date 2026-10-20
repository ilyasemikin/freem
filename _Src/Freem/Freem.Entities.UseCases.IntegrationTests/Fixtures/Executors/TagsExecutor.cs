using Freem.Domain.Entities.Tags;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Executors;

public sealed class TagsExecutor
{
    private readonly IRequestMediator<HandlerContext> _mediator;

    public TagsExecutor(IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    public async Task<Tag> CreateAsync(HandlerContext context, CreateTagRequest request)
    {
        var result = await _mediator.ExecuteAsync<CreateTagRequest, Result<CreateTagResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.Tag;
    }

    public async Task<Tag> CreateAsync(HandlerContext context)
    {
        var value = Guid.NewGuid().ToString();
        var name = TagName.Parse(value, null);

        var request = new CreateTagRequest(name);
        return await CreateAsync(context, request);
    }

    public async Task<IReadOnlyList<Tag>> CreateManyAsync(HandlerContext context, int count)
    {
        var tags = new List<Tag>();
        foreach (var _ in Enumerable.Range(0, count))
        {
            var tag = await CreateAsync(context);
            tags.Add(tag);
        }

        return tags;
    }

    public async Task<Tag> RequiredGetAsync(HandlerContext context, TagId id)
    {
        var request = new GetTagRequest(id);
        var result = await _mediator.ExecuteAsync<GetTagRequest, Result<GetTagResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.Tag;
    }
}