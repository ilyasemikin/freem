using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer;

public sealed class ApiListFormatsRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.ListFormats;
}
