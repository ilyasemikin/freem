using Freem.Domain.Storage.Models.Filters.Models;

namespace Freem.Domain.Storage.Models.Filters.Abstractions;

public interface IOffsetFilter : IFilter
{
    Offset Offset { get; }
}