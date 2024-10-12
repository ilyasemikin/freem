using Freem.Entities.UseCases.Models.Filter;
using Limit = Freem.Entities.Storage.Abstractions.Models.Filters.Models.Limit;

namespace Freem.Entities.UseCases.Tags.List.Models;

public sealed class ListTagRequest
{
    public Limit Limit { get; init; }
    public Offset Offset { get; init; }
}