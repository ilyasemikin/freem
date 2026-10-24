using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Formats.List;

public sealed class ListFormatsRequest : IRequest
{
    public RequestType Type => RequestTypes.Formats.List;
}
