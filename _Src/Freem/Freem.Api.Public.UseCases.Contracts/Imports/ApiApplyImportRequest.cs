using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiApplyImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Apply;

    public ImportDocumentId Id { get; }
    public ApplyImportRequest Body { get; }

    public ApiApplyImportRequest(ImportDocumentId id, ApplyImportRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}
