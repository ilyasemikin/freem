using Freem.Api.Public.Contracts.DTO.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiCreateRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Create;

    public CreateRecordRequest Body { get; }

    public ApiCreateRecordRequest(CreateRecordRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
