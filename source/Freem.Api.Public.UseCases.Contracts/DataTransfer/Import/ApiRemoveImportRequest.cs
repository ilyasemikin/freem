using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiRemoveImportRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.Remove;

    public ImportDocumentId Id { get; }

    public ApiRemoveImportRequest(ImportDocumentId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
