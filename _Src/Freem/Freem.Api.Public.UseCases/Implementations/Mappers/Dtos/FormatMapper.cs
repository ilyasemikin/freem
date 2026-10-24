using Freem.Api.Public.Contracts.V1.DataTransfer.Dtos;
using Freem.Api.Public.Contracts.V1.DataTransfer;
using Freem.Domain.Formats;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class FormatMapper
{
    public static FormatDto Map(this FormatDescription format)
    {
        return new FormatDto(format.Key, format.Name, format.FileExtension, format.MediaType);
    }
}
