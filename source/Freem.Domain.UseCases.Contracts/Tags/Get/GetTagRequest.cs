using Freem.Domain.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Tags.Get;

public sealed class GetTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Tags.Get;

    public TagId Id { get; }

    public GetTagRequest(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}