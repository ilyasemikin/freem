using Freem.Domain.Records;
using Freem.Web.Api.Public.Contracts.DTO.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Records;

public sealed class ApiUpdateRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Update;

    public RecordId Id { get; }
    public UpdateRecordRequest Body { get; }

    public ApiUpdateRecordRequest(RecordId id, UpdateRecordRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}
