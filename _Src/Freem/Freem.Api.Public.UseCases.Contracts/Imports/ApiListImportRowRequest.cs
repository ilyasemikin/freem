using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiListImportRowRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.ListRows;

    public ImportDocumentId Id { get; }
    public ListImportRowRequest Query { get; }

    public ApiListImportRowRequest(ImportDocumentId id, ListImportRowRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
