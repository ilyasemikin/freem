using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiUpdateImportTagMappingRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.UpdateTagMapping;

    public ImportDocumentId Id { get; }
    public ImportTagId ImportTagId { get; }
    public ImportTagMappingUpdateRequest Body { get; }

    public ApiUpdateImportTagMappingRequest(ImportDocumentId id, ImportTagId importTagId, ImportTagMappingUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(importTagId);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        ImportTagId = importTagId;
        Body = body;
    }
}
