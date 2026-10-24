using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiUpdateImportActivityMappingRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.UpdateActivityMapping;

    public ImportDocumentId Id { get; }
    public ImportActivityId ImportActivityId { get; }
    public UpdateImportActivityMappingRequest Body { get; }

    public ApiUpdateImportActivityMappingRequest(ImportDocumentId id, ImportActivityId importActivityId, UpdateImportActivityMappingRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(importActivityId);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        ImportActivityId = importActivityId;
        Body = body;
    }
}
