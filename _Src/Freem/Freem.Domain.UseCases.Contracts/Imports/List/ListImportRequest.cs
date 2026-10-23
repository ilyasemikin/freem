using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Imports.List;

public sealed class ListImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.List;

    public Limit Limit { get; }
    public Offset Offset { get; }

    public ListImportRequest()
        : this(Limit.DefaultValue, Offset.DefaultValue)
    {
    }

    public ListImportRequest(Limit limit, Offset offset)
    {
        Limit = limit;
        Offset = offset;
    }
}
