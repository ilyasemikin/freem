using Freem.Api.Public.Contracts.V1.DataTransfer.Formats.Dtos;
using Freem.Api.Public.Contracts.V1.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Formats;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos.DataTransfer;

internal static class FormatMapper
{
    public static FormatDto Map(this FormatDescription format)
    {
        return new FormatDto(format.Key, format.Name, format.FileExtension, format.MediaType);
    }
}
