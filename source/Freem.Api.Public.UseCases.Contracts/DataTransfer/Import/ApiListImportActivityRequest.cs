using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiListImportActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.ListActivities;

    public ImportDocumentId Id { get; }
    public ImportActivityListRequest Query { get; }

    public ApiListImportActivityRequest(ImportDocumentId id, ImportActivityListRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
