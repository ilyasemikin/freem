using Freem.Domain.Storage.Models.Filters.Models;

namespace Freem.Domain.Storage.Models.Filters.Abstractions;

public interface ILimitFilter : IFilter
{
    Limit Limit { get; }
}