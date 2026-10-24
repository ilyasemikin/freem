using Freem.Api.Public.Contracts.V1.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiUpdateImportActivityMappingRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.UpdateActivityMapping;

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
