using Freem.Api.Public.UseCases.Contracts.DataTransfer.Formats;
using Freem.Domain.UseCases.Contracts.DataTransfer.Formats.List;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests.DataTransfer;

internal static class FormatsRequestMapper
{
    public static ListFormatsRequest Map(this ApiListFormatsRequest request)
    {
        return new ListFormatsRequest();
    }
}
