using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Activities.List.Models;

public sealed class ListActivityRequest
{
    public Limit Limit { get; init; }
    public Offset Offset { get; init; }
}