using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Formats.List;

public sealed class ListFormatsRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Formats.List;
}
