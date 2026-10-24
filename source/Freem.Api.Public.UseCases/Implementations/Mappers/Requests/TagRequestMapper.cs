using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Domain.UseCases.Contracts.Tags.Get;
using Freem.Domain.UseCases.Contracts.Tags.Search;
using Freem.Domain.UseCases.Contracts.Tags.List;
using Freem.Domain.UseCases.Contracts.Tags.Remove;
using Freem.Domain.UseCases.Contracts.Tags.Update;
using Freem.Api.Public.UseCases.Contracts.Tags;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class TagRequestMapper
{
    public static CreateTagRequest Map(this ApiCreateTagRequest request)
    {
        return new CreateTagRequest(request.Body.Name);
    }

    public static SearchTagByNameRequest Map(this ApiSearchTagByNameRequest request)
    {
        var searchText = request.Name.ToString();
        return new SearchTagByNameRequest(searchText);
    }

    public static GetTagRequest Map(this ApiGetTagRequest request)
    {
        return new GetTagRequest(request.Id);
    }

    public static ListTagRequest Map(this ApiListTagRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListTagRequest(limit, offset);
    }

    public static RemoveTagRequest Map(this ApiRemoveTagRequest request)
    {
        return new RemoveTagRequest(request.Id);
    }

    public static UpdateTagRequest Map(this ApiUpdateTagRequest request)
    {
        return new UpdateTagRequest(request.Id)
        {
            Name = request.Body.Name?.Map()
        };
    }
}
