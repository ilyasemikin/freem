using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiApplyImportRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.Apply;

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
