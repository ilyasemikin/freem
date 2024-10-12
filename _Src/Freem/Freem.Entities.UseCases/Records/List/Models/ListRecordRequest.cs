using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Records.List.Models;

public sealed class ListRecordRequest
{
    public Limit Limit { get; init; }
    public Offset Offset { get; init; }
}