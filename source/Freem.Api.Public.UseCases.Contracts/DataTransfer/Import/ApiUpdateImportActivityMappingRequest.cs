using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiUpdateImportActivityMappingRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.UpdateActivityMapping;

    public ImportDocumentId Id { get; }
    public ImportActivityId ImportActivityId { get; }
    public ImportActivityMappingUpdateRequest Body { get; }

    public ApiUpdateImportActivityMappingRequest(ImportDocumentId id, ImportActivityId importActivityId, ImportActivityMappingUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(importActivityId);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        ImportActivityId = importActivityId;
        Body = body;
    }
}
