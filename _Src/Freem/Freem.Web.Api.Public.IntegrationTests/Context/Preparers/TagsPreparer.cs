using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.IntegrationTests.Extensions;

namespace Freem.Web.Api.Public.IntegrationTests.Context.Preparers;

public sealed class TagsPreparer
{
    private readonly TestContext _context;

    public TagsPreparer(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    public async Task<TagId> CreateAsync(string name = "name0")
    {
        var tagName = TagName.Parse(name, null);
        var request = new CreateTagRequest(tagName);
        var response = await _context.Client.Tags.CreateAsync(request);

        return response.Unwrap().Id;
    }

    public async Task<IReadOnlyList<TagId>> CreateManyAsync(int count)
    {
        var ids = new TagId[count];
        for (var i = 0; i < count; i++)
        {
            var name = $"name{i}";
            ids[i] = await CreateAsync(name);
        }

        return ids;
    }
}
