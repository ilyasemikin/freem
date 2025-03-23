using Freem.Entities.Tags.Identifiers;
using Freem.Web.Api.Public.Contracts.Tags;

namespace Freem.Web.Api.Public.FunctionalTests.Context.Preparers;

public sealed class TagsPreparer
{
    private readonly TestContext _context;

    public TagsPreparer(TestContext context)
    {
        _context = context;
    }

    public TagIdentifier Create(string name = "name0")
    {
        var trq = new CreateTagRequest(name);
        var trs = _context.SyncClient.Tags.Create(trq);
        trs.EnsureSuccess();

        return trs.Value.Id;
    }

    public IEnumerable<TagIdentifier> CreateMany(int count)
    {
        var ids = new TagIdentifier[count];

        for (var i = 0; i < count; i++)
        {
            var name = $"name{i}";
            var request = new CreateTagRequest(name);
            var response = _context.SyncClient.Tags.Create(request);
            response.EnsureSuccess();

            ids[i] = response.Value.Id;
        }

        return ids;
    }
}