using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiUpdateImportTagMappingRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.UpdateTagMapping;

    public ImportDocumentId Id { get; }
    public ImportTagId ImportTagId { get; }
    public UpdateImportTagMappingRequest Body { get; }

    public ApiUpdateImportTagMappingRequest(ImportDocumentId id, ImportTagId importTagId, UpdateImportTagMappingRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(importTagId);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        ImportTagId = importTagId;
        Body = body;
    }
}
