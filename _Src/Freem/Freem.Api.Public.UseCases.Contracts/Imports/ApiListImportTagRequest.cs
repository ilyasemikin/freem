using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiListImportTagRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.ListTags;

    public ImportDocumentId Id { get; }
    public ListImportTagRequest Query { get; }

    public ApiListImportTagRequest(ImportDocumentId id, ListImportTagRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
