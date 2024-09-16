using Freem.Entities.Storage.Abstractions.Models.Filters.Models;

namespace Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;

public interface IOffsetFilter
{
    Offset Offset { get; }
}