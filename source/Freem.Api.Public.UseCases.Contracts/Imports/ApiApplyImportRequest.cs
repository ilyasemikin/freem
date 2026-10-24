using Freem.Api.Public.Contracts.V1.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiApplyImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Apply;

    public ImportDocumentId Id { get; }
    public ImportApplyRequest Body { get; }

    public ApiApplyImportRequest(ImportDocumentId id, ImportApplyRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}
