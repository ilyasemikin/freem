using Freem.Entities.Storage.Abstractions.Models.Filters.Models;

namespace Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;

public interface ILimitFilter : IFilter
{
    Limit Limit { get; }
}