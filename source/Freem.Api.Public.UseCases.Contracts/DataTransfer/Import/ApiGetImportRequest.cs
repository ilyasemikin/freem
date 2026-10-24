using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiGetImportRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.Get;

    public ImportDocumentId Id { get; }

    public ApiGetImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
