using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Web.Api.Public.Contracts.DTO.Tags;

namespace Freem.Web.Api.Public.FunctionalTests.Context.Preparers;

public sealed class TagsPreparer
{
    private readonly TestContext _context;

    public TagsPreparer(TestContext context)
    {
        _context = context;
    }

    public TagId Create(string name = "name0")
    {
        var tagName = TagName.Parse(name, null);
        var trq = new CreateTagRequest(tagName);
        var trs = _context.SyncClient.Tags.Create(trq);
        trs.EnsureSuccess();

        return trs.Value.Id;
    }

    public IEnumerable<TagId> CreateMany(int count)
    {
        var ids = new TagId[count];

        for (var i = 0; i < count; i++)
        {
            var name = $"name{i}";
            var tagName = TagName.Parse(name, null);
            var request = new CreateTagRequest(tagName);
            var response = _context.SyncClient.Tags.Create(request);
            response.EnsureSuccess();

            ids[i] = response.Value.Id;
        }

        return ids;
    }
}