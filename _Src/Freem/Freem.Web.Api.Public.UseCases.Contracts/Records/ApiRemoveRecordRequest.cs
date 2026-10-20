using Freem.Domain.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Records;

public sealed class ApiRemoveRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Remove;

    public RecordId Id { get; }

    public ApiRemoveRecordRequest(RecordId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
